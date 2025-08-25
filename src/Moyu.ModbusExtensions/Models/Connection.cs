// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Moyu.LogExtensions.LogHelpers;
using NModbus;

namespace Moyu.ModbusExtensions.Models;

internal sealed class Connection : IAsyncDisposable
{
    internal TcpClient Client { get; }
    public IModbusMaster Master { get; }
    public bool IsHealthy { get; set; } = true;

    private Connection(TcpClient client, IModbusMaster master)
    {
        Client = client;
        Master = master;
    }

    /// <summary>
    /// 建立连接（异步，带超时）
    /// </summary>
    public static async Task<Connection> CreateAsync(
        ModbusFactory factory,
        string host,
        int port,
        TimeSpan timeout,
        ILogger logger,
        CancellationToken? token = null
    )
    {
        TcpClient client = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true };
        SetKeepAliveCrossPlatform(client.Client, true, logger);
        using CancellationTokenSource cts = new CancellationTokenSource(timeout);

        try
        {
            await client.ConnectAsync(host, port, token ?? cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            client.Dispose();
            string message = $"连接 {host}:{port} 超时({timeout.TotalSeconds} 秒)";
            logger.Error(message);
            throw new TimeoutException(message);
        }
        catch
        {
            client.Dispose();
            throw;
        }

        IModbusMaster? master = factory.CreateMaster(client);
        return new(client, master);
    }

    private static void SetKeepAliveCrossPlatform(
        Socket socket,
        bool enable,
        ILogger logger,
        uint keepAliveTimeMs = 5000,
        uint keepAliveIntervalMs = 1000
    )
    {
        try
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            if (OperatingSystem.IsWindows())
            {
                byte[] inOptionValues = new byte[12];
                BitConverter.GetBytes(enable ? 1u : 0u).CopyTo(inOptionValues, 0);
                BitConverter.GetBytes(keepAliveTimeMs).CopyTo(inOptionValues, 4);
                BitConverter.GetBytes(keepAliveIntervalMs).CopyTo(inOptionValues, 8);
                socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            }
            else
            {
                // Linux/macOS 无法精确控制时间，只能开启 KeepAlive
                // 内核会使用系统默认的 keepalive_time 和 keepalive_interval
            }
        }
        catch (PlatformNotSupportedException)
        {
            // 安全回退：忽略，仍然可以连接
        }
        catch (Exception ex)
        {
            // 记录日志，但不阻塞连接
            logger.Warn($"设置 KeepAlive 失败: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        Master.Dispose();
        Client.Dispose();
        await Task.CompletedTask;
    }

    public override string ToString()
    {
        return $"{Client.Client.RemoteEndPoint}|{Client.Client.LocalEndPoint}";
    }
}
