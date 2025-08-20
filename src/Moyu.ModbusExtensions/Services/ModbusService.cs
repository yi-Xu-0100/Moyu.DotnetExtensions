// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moyu.LogExtensions.LogHelpers;
using Moyu.ModbusExtensions.Models;
using NModbus;

namespace Moyu.ModbusExtensions.Services;

/// <summary>
/// 提供 Modbus 通信服务，包括连接池、请求执行和轮询组管理。
/// </summary>
public class ModbusService : IHostedService, IAsyncDisposable
{
    private readonly ModbusFactory _factory = new();
    private readonly string _host;
    private readonly int _port;
    private readonly int _maxConnections;
    private readonly TimeSpan _connectTimeout;
    private readonly SemaphoreSlim _requestThrottler;
    private readonly ConcurrentDictionary<string, PollingGroup> _pollingGroups = new();
    private readonly ILogger<ModbusService> _logger;
    private readonly ILogger<ConnectionPool> _poolLogger;
    private readonly ILogger<Connection> _connectionLogger;
    private ConnectionPool _connectionPool = null!;

    /// <summary>
    /// 初始化 ModbusService 实例。
    /// </summary>
    /// <param name="logger">日志记录器</param>
    /// <param name="poolLogger">连接池日志记录器</param>
    /// <param name="host">Modbus 服务器主机</param>
    /// <param name="port">Modbus 端口，默认502</param>
    /// <param name="maxConnections">最大连接数</param>
    /// <param name="maxConcurrentRequests">最大并发请求数</param>
    /// <param name="connectTimeoutSeconds">连接超时时间（秒）</param>
    internal ModbusService(
        ILogger<ModbusService> logger,
        ILogger<ConnectionPool> poolLogger,
        ILogger<Connection> connectionLogger,
        string host,
        int port = 502,
        int maxConnections = 5,
        int maxConcurrentRequests = 10,
        int connectTimeoutSeconds = 5
    )
    {
        _logger = logger;
        _poolLogger = poolLogger;
        _connectionLogger = connectionLogger;
        _host = host;
        _port = port;
        _maxConnections = maxConnections;
        _connectTimeout = TimeSpan.FromSeconds(connectTimeoutSeconds);
        _requestThrottler = new(maxConcurrentRequests);
    }

    #region IHostedService

    /// <summary>
    /// 启动服务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connectionPool = new(_factory, _host, _port, _connectTimeout, _poolLogger, _connectionLogger, _maxConnections);
        _logger.Info("ModbusService 已启动");
        await Task.CompletedTask;
    }

    /// <summary>
    /// 停止服务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (PollingGroup group in _pollingGroups.Values)
        {
            await group.Cts.CancelAsync();
            group.Dispose();
        }
        _pollingGroups.Clear();

        await _connectionPool.DisposeAsync();

        _logger.Info("ModbusService 已停止");
    }

    #endregion

    #region 前台请求

    /// <summary>
    /// 执行 Modbus 请求（无返回值）。
    /// </summary>
    /// <param name="request">请求委托</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task ExecuteRequestAsync(
        Func<IModbusMaster, Task> request,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? cancellationToken = null
    )
    {
        CancellationToken token = cancellationToken ?? CancellationToken.None;
        TimeSpan timeout = waitTimeout ?? TimeSpan.FromSeconds(5);

        if (!await _requestThrottler.WaitAsync(timeout, token))
        {
            throw new TimeoutException($"等待前台请求执行({nameof(_requestThrottler)})超过{timeout.TotalSeconds}s");
        }

        Connection? conn = null;
        try
        {
            conn = await _connectionPool.GetConnectionAsync(token);
            if (maxRetries > 1)
            {
                await ExecuteWithRetry(() => request(conn.Master), maxRetries, token, reTryTimeSpan);
            }
            else
            {
                await request(conn.Master);
            }
        }
        catch
        {
            if (conn != null)
                conn.IsHealthy = false;
            throw;
        }
        finally
        {
            if (conn != null)
                _connectionPool.Return(conn);
            _requestThrottler.Release();
        }
    }

    /// <summary>
    /// 执行 Modbus 请求（带返回值）。
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="request">请求委托</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>请求结果</returns>
    public async Task<T> ExecuteRequestAsync<T>(
        Func<IModbusMaster, Task<T>> request,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        CancellationToken cancellationToken = token ?? CancellationToken.None;
        TimeSpan timeout = waitTimeout ?? TimeSpan.FromSeconds(5);

        if (!await _requestThrottler.WaitAsync(timeout, cancellationToken))
            throw new TimeoutException($"等待前台请求执行({nameof(_requestThrottler)})超过{timeout.TotalSeconds}s");

        Connection? conn = null;
        try
        {
            conn = await _connectionPool.GetConnectionAsync(cancellationToken);
            if (maxRetries > 1)
            {
                return await ExecuteWithRetry(() => request(conn.Master), maxRetries, cancellationToken, reTryTimeSpan);
            }
            else
            {
                return await request(conn.Master);
            }
        }
        catch
        {
            if (conn != null)
                conn.IsHealthy = false;
            throw;
        }
        finally
        {
            if (conn != null)
                _connectionPool.Return(conn);
            _requestThrottler.Release();
        }
    }

    #endregion

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
        _logger.Info($"在轮询组 {groupId} 添加任务 {task.Method.Name}");
        return true;
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

    #region 核心重试

    /// <summary>
    /// 带重试机制的异步操作执行（无返回值）。
    /// </summary>
    /// <param name="action">操作委托</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="ct">取消令牌</param>
    /// <param name="retryTimeSpan">重试间隔</param>
    private async Task ExecuteWithRetry(
        Func<Task> action,
        int maxRetries,
        CancellationToken ct,
        TimeSpan? retryTimeSpan = null
    )
    {
        int attempt = 0;
        while (attempt++ < maxRetries)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                await action();
                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                _logger.Warn(ex, $"未成功请求, 等待2s后重试");
                await Task.Delay(retryTimeSpan ?? TimeSpan.FromSeconds(2), ct);
            }
            catch (Exception ex) when (attempt >= maxRetries)
            {
                string message = $"操作失败，已达最大重试次数 {maxRetries}, {ex.Message}";
                _logger.Error(ex, message);
                throw new InvalidOperationException(message);
            }
        }
        throw new InvalidCastException($"操作失败, 请求异常退出");
    }

    /// <summary>
    /// 带重试机制的异步操作执行（带返回值）。
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="action">操作委托</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="ct">取消令牌</param>
    /// <param name="retryTimeSpan">重试间隔</param>
    /// <returns>操作结果</returns>
    private async Task<T> ExecuteWithRetry<T>(
        Func<Task<T>> action,
        int maxRetries,
        CancellationToken ct,
        TimeSpan? retryTimeSpan = null
    )
    {
        int attempt = 0;
        while (attempt++ < maxRetries)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                return await action();
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                _logger.Warn(ex, $"未成功请求, 等待2s后重试");
                await Task.Delay(retryTimeSpan ?? TimeSpan.FromSeconds(2), ct);
            }
            catch (Exception ex) when (attempt >= maxRetries)
            {
                string message = $"操作失败, 已达最大重试次数 {maxRetries}, {ex.Message}";
                _logger.Error(ex, message);
                throw new InvalidOperationException(message);
            }
        }
        throw new InvalidCastException($"操作失败, 请求异常退出");
    }

    #endregion

    #region DisposeAsync

    /// <summary>
    /// 释放资源。
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        foreach (PollingGroup group in _pollingGroups.Values)
        {
            await group.Cts.CancelAsync();
            group.Dispose();
        }
        _pollingGroups.Clear();

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_connectionPool != null)
        {
            await _connectionPool.DisposeAsync();
        }
        _requestThrottler.Dispose();
    }

    #endregion
}
