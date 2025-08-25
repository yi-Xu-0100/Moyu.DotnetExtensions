// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 写入一个 Double 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">double 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteDoubleHoldingRegisterAsync(
        ushort startAddress,
        double value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteDoubleHoldingRegisterAsync(_slaveAddress, startAddress, value, _endian),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 写入多个 Double 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">double 类型保持寄存器值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteDoubleHoldingRegistersAsync(
        ushort startAddress,
        double[] values,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteDoubleHoldingRegistersAsync(_slaveAddress, startAddress, values, _endian),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
