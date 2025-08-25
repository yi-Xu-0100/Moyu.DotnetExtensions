// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Net.Sockets;
using Moyu.LogExtensions.LogHelpers;
using Moyu.ModbusExtensions.Exceptions;
using Moyu.ModbusExtensions.Models;
using NModbus;

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    #region 轮询组

    /// <summary>
    /// 创建轮询组。
    /// </summary>
    /// <param name="groupId">组ID</param>
    /// <param name="interval">轮询间隔</param>
    /// <param name="retries">重试次数</param>
    /// <param name="retryInterval">重试间隔</param>
    public void CreatePollingGroup(string groupId, TimeSpan interval, int retries = 3, TimeSpan? retryInterval = null)
    {
        if (!_pollingGroups.TryAdd(groupId, new(groupId, interval, retries, retryInterval)))
        {
            return;
        }

        StartPollingLoop(groupId);
        _logger.Info($"创建轮询组 {groupId}");
    }

    /// <summary>
    /// 向轮询组添加任务。
    /// </summary>
    /// <param name="groupId">组ID</param>
    /// <param name="task">任务委托</param>
    /// <returns>添加是否成功</returns>
    public bool AddPollingTask(string groupId, Func<IModbusMaster, Task> task)
    {
        if (!_pollingGroups.TryGetValue(groupId, out PollingGroup? group))
        {
            return false;
        }

        group.AddTask(task);
        _logger.Info($"在轮询组 {groupId} 添加任务 {task.Method.Name}, 总共{group.TaskCount}个任务");
        return true;
    }

    /// <summary>
    /// 关闭全部轮询组
    /// </summary>
    public async Task StopPollingLoopAsync()
    {
        foreach (PollingGroup group in _pollingGroups.Values)
        {
            try
            {
                await group.Cts.CancelAsync();
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "取消PollingGroup失败");
            }

            try
            {
                group.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "释放PollingGroup资源失败");
            }
        }

        _pollingGroups.Clear();
    }

    /// <summary>
    /// 启动轮询循环。
    /// </summary>
    /// <param name="groupId">组ID</param>
    private void StartPollingLoop(string groupId)
    {
        if (!_pollingGroups.TryGetValue(groupId, out PollingGroup? group))
            return;

        _ = Task.Run(
            async () =>
            {
                while (!group.Cts.IsCancellationRequested)
                {
                    Connection? conn = null;
                    try
                    {
                        conn = await _connectionPool.GetConnectionAsync(group.Cts.Token);
                        List<Func<IModbusMaster, Task>> tasks = group.GetTasksSnapshot();
                        foreach (Func<IModbusMaster, Task> task in tasks)
                        {
                            await ExecuteWithRetry(
                                () => task(conn.Master),
                                group.RetryCount,
                                group.Cts.Token,
                                group.RetryInterval
                            );
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex) when (ex is MaxRetryException maxRetryEx)
                    {
                        _logger.Error($"轮询组 {groupId} 执行失败, {maxRetryEx.Message}");
                        if (conn != null)
                            conn.IsHealthy = false;
                    }
                    catch (Exception ex) when (ex is SocketException socketEx)
                    {
                        _logger.Error($"轮询组 {groupId} 执行失败[{socketEx.ErrorCode}], {socketEx.Message}");
                        if (conn != null)
                            conn.IsHealthy = false;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"轮询组 {groupId} 执行失败");
                        if (conn != null)
                            conn.IsHealthy = false;
                    }
                    finally
                    {
                        if (conn != null)
                            _connectionPool.Return(conn);
                    }

                    try
                    {
                        await Task.Delay(group.Interval, group.Cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }

                _logger.Info($"轮询组 {groupId} 已退出");
            },
            group.Cts.Token
        );
    }
    #endregion
}
