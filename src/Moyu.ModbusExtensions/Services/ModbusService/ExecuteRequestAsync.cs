// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using Moyu.ModbusExtensions.Models;
using NModbus;

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
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
}
