// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions.Models;

internal sealed class PollingGroup(string id, TimeSpan interval, int retryCount, TimeSpan? retryInterval) : IDisposable
{
    public CancellationTokenSource Cts { get; } = new();
    public TimeSpan Interval { get; } = interval;
    public int RetryCount { get; } = retryCount;

    public TimeSpan? RetryInterval { get; } = retryInterval;

    private readonly List<Func<IModbusMaster, Task>> _tasks = [];
    private readonly object _lock = new();

    public string Id { get; } = id;

    public void AddTask(Func<IModbusMaster, Task> task)
    {
        lock (_lock)
        {
            _tasks.Add(task);
        }
    }

    public List<Func<IModbusMaster, Task>> GetTasksSnapshot()
    {
        lock (_lock)
        {
            return [.. _tasks];
        }
    }

    public void Dispose() => Cts.Dispose();
}
