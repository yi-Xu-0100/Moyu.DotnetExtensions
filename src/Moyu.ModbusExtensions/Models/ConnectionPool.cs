// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Moyu.LogExtensions.LogHelpers;
using NModbus;

namespace Moyu.ModbusExtensions.Models;

internal sealed class ConnectionPool : IAsyncDisposable
{
    private readonly ModbusFactory _factory;
    private readonly string _host;
    private readonly int _port;
    private readonly TimeSpan _timeout;
    private readonly int _maxSize;
    private readonly TimeSpan _idleLifetime;
    private readonly ConcurrentBag<PooledConnection> _pool = [];
    private readonly ILogger<ConnectionPool> _logger;
    private readonly ILogger<Connection> _connectionLogger;

    private int _currentSize;
    private readonly Timer _cleanupTimer;

    public ConnectionPool(
        ModbusFactory factory,
        string host,
        int port,
        TimeSpan timeout,
        ILogger<ConnectionPool> logger,
        ILogger<Connection> connectionLogger,
        int maxSize = 10,
        TimeSpan? idleLifetime = null
    )
    {
        _factory = factory;
        _host = host;
        _port = port;
        _timeout = timeout;
        _logger = logger;
        _connectionLogger = connectionLogger;
        _maxSize = maxSize;
        _idleLifetime = idleLifetime ?? TimeSpan.FromMinutes(1);

        // 定时清理 (每 30 秒跑一次)
        _cleanupTimer = new(
            async void (_) =>
            {
                try
                {
                    await CleanupAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error cleaning up connection");
                }
            },
            null,
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(30)
        );
    }

    /// <summary>
    /// 获取连接
    /// </summary>
    public async Task<Connection> GetConnectionAsync(CancellationToken? token = null)
    {
        var cancellationToken = token ?? CancellationToken.None;
        // 优先取可用的
        while (_pool.TryTake(out PooledConnection? pooled))
        {
            if (pooled.Conn is { IsHealthy: true, Client.Connected: true })
            {
                // 尝试 Modbus 层 ping
                if (await TestConnectionAsync(pooled.Conn.Master))
                {
                    pooled.Touch();
                    return pooled.Conn;
                }

                _logger.Warn($"连接 {pooled.Conn} 无效，已丢弃");
                pooled.Conn.IsHealthy = false;
                await pooled.Conn.DisposeAsync().ConfigureAwait(false);
                Interlocked.Decrement(ref _currentSize);
            }

            await pooled.Conn.DisposeAsync().ConfigureAwait(false);
            Interlocked.Decrement(ref _currentSize);
        }

        // 没有可用就新建
        if (Interlocked.Increment(ref _currentSize) <= _maxSize)
        {
            try
            {
                Connection conn = await Connection
                    .CreateAsync(_factory, _host, _port, _timeout, _connectionLogger, cancellationToken)
                    .ConfigureAwait(false);
                return conn;
            }
            catch
            {
                Interlocked.Decrement(ref _currentSize);
                throw;
            }
        }

        // 已到上限，等待别人归还
        while (true)
        {
            await Task.Delay(50, cancellationToken).ConfigureAwait(false);

            if (!_pool.TryTake(out PooledConnection? pooled) || !pooled.Conn.IsHealthy)
            {
                continue;
            }

            pooled.Touch();
            return pooled.Conn;
        }
    }

    /// <summary>
    /// 归还连接
    /// </summary>
    public void Return(Connection conn)
    {
        if (conn.IsHealthy)
            _pool.Add(new(conn));
        else
        {
            conn.DisposeAsync().AsTask().ConfigureAwait(false);
            Interlocked.Decrement(ref _currentSize);
        }
    }

    /// <summary>
    /// 定期清理空闲连接
    /// </summary>
    private async Task CleanupAsync()
    {
        DateTime cutoff = DateTime.UtcNow - _idleLifetime;
        List<PooledConnection> survivors = [];

        while (_pool.TryTake(out PooledConnection? pooled))
        {
            if (pooled.LastUsed < cutoff)
            {
                await pooled.Conn.DisposeAsync().ConfigureAwait(false);
                Interlocked.Decrement(ref _currentSize);
            }
            else
            {
                survivors.Add(pooled);
            }
        }

        // 放回未过期的
        foreach (PooledConnection s in survivors)
        {
            _pool.Add(s);
        }
    }

    /// <summary>
    /// 测试 Modbus 连接是否可用
    /// </summary>
    private static async Task<bool> TestConnectionAsync(IModbusMaster master)
    {
        try
        {
            await master.ReadCoilsAsync(0, 1, 0); // 测试读取第 0 号线圈
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _cleanupTimer.DisposeAsync();
        while (_pool.TryTake(out PooledConnection? pooled))
        {
            await pooled.Conn.DisposeAsync().ConfigureAwait(false);
        }
    }
}

internal sealed class PooledConnection
{
    public Connection Conn { get; }
    public DateTime LastUsed { get; private set; }

    public PooledConnection(Connection conn)
    {
        Conn = conn;
        Touch();
    }

    public void Touch() => LastUsed = DateTime.UtcNow;
}
