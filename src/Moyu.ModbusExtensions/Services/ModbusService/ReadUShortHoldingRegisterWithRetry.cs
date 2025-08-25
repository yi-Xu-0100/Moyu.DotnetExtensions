// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 读取一个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>ushort 值</returns>
    public async Task<ushort> ReadHoldingRegisterAsync(
        ushort startAddress,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master => master.ReadHoldingRegisterAsync(_slaveAddress, startAddress),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
    /// <summary>
    /// 读取多个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>ushort 数组</returns>
    public async Task<ushort[]> ReadHoldingRegistersAsync(
        ushort startAddress,
        ushort numberOfHoldingRegisters,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master => master.ReadHoldingRegistersAsync(_slaveAddress, startAddress, numberOfHoldingRegisters),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
