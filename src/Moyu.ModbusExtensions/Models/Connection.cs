// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Net.Sockets;
using NModbus;

namespace Moyu.ModbusExtensions.Models;

internal sealed class Connection : IAsyncDisposable
{
    private TcpClient Client { get; }
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
        CancellationToken? token = null
    )
    {
        TcpClient client = new TcpClient { NoDelay = true };
        using CancellationTokenSource cts = new CancellationTokenSource(timeout);

        try
        {
            await client.ConnectAsync(host, port, token ?? cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            client.Dispose();
            throw new TimeoutException($"连接 {host}:{port} 超时({timeout.TotalSeconds} 秒)");
        }
        catch
        {
            client.Dispose();
            throw;
        }

        IModbusMaster? master = factory.CreateMaster(client);
        return new(client, master);
    }

    public async ValueTask DisposeAsync()
    {
        Master.Dispose();
        Client.Dispose();
        await Task.CompletedTask;
    }
}
