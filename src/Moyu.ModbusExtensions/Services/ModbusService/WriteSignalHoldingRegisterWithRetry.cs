// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 写入一个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">bool 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteSignalHoldingRegisterAsync(
        ushort startAddress,
        bool value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteSignalHoldingRegisterAsync(_slaveAddress, startAddress, value),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 写入多个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">bool 类型保持寄存器值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteSignalHoldingRegistersAsync(
        ushort startAddress,
        bool[] values,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteSignalHoldingRegistersAsync(_slaveAddress, startAddress, values),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
    /// <summary>
    /// 写入 Boolean 类型的一个寄存器(16 boolean => 1 register)
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">bool 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteBitSignalHoldingRegisterAsync(
        ushort startAddress,
        bool[] value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteBitSignalHoldingRegisterAsync(_slaveAddress, startAddress, value),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 写入 Boolean 类型到多个保持寄存器 (16 boolean => 1 register)
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">bool 类型保持寄存器值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteBitSignalHoldingRegistersAsync(
        ushort startAddress,
        bool[] values,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteBitSignalHoldingRegistersAsync(_slaveAddress, startAddress, values),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
