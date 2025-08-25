// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 写入一个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">ushort 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteHoldingRegisterAsync(
        ushort startAddress,
        ushort value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteSingleRegisterAsync(_slaveAddress, startAddress, value),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 写入多个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">ushort 值</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    public async Task WriteHoldingRegistersAsync(
        ushort startAddress,
        ushort[] value,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        await ExecuteRequestAsync(
            master => master.WriteMultipleRegistersAsync(_slaveAddress, startAddress, value),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
