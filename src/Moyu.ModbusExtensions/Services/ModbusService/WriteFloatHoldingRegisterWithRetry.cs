// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 写入一个 float 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">float 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteFloatHoldingRegisterAsync(
        ushort startAddress,
        float value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteFloatHoldingRegisterAsync(_slaveAddress, startAddress, value, _endian),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 写入多个 float 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">float 类型保持寄存器值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteFloatHoldingRegistersAsync(
        ushort startAddress,
        float[] values,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteFloatHoldingRegistersAsync(_slaveAddress, startAddress, values, _endian),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
