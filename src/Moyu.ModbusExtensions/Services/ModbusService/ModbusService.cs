// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Collections.Concurrent;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moyu.LogExtensions.LogHelpers;
using Moyu.ModbusExtensions.Exceptions;
using Moyu.ModbusExtensions.Models;
using NModbus;

namespace Moyu.ModbusExtensions.Services;

/// <summary>
/// 提供 Modbus 通信服务，包括连接池、请求执行和轮询组管理。
/// </summary>
public partial class ModbusService : IHostedService, IAsyncDisposable
{
    private readonly ModbusFactory _factory = new();
    private readonly string _host;
    private readonly int _port;
    private readonly int _maxConnections;
    private readonly TimeSpan _connectTimeout;
    private readonly SemaphoreSlim _requestThrottler;
    private readonly ConcurrentDictionary<string, PollingGroup> _pollingGroups = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<ModbusService> _logger;
    private readonly byte _slaveAddress;
    private readonly ModbusEndian _endian;
    private ConnectionPool _connectionPool = null!;

    /// <summary>
    /// 初始化 ModbusService 实例。
    /// </summary>
    /// <param name="loggerFactory">日志记录工厂接口</param>
    /// <param name="host">Modbus 服务器主机</param>
    /// <param name="port">Modbus 端口，默认502</param>
    /// <param name="slaveAddress">Modbus 从机设备地址</param>
    /// <param name="maxConnections">最大连接数, 默认5</param>
    /// <param name="maxConcurrentRequests">最大并发请求数, 默认10</param>
    /// <param name="connectTimeoutSeconds">连接超时时间(s), 默认5</param>
    public ModbusService(
        ILoggerFactory loggerFactory,
        string host,
        int port = 502,
        byte slaveAddress = 1,
        ModbusEndian endian = ModbusEndian.ABCD,
        int maxConnections = 5,
        int maxConcurrentRequests = 10,
        int connectTimeoutSeconds = 5
    )
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<ModbusService>();
        _host = host;
        _port = port;
        _slaveAddress = slaveAddress;
        _endian = endian;
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
        _connectionPool = new(_factory, _host, _port, _slaveAddress, _connectTimeout, _loggerFactory, _maxConnections);
        _logger.Info("ModbusService 已启动");
        await Task.CompletedTask;
    }

    /// <summary>
    /// 停止服务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await StopPollingLoopAsync();

        await _connectionPool.DisposeAsync();

        _logger.Info("ModbusService 已停止");
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
        while (attempt++ <= maxRetries)
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
                if (ex is IOException && ex.InnerException is SocketException socketEx)
                {
                    message = $"操作失败[{socketEx.ErrorCode}]，已达最大重试次数 {maxRetries}, {ex.Message}";
                }
                else
                {
                    _logger.Error(ex, message);
                }
                throw new MaxRetryException(3, message);
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
