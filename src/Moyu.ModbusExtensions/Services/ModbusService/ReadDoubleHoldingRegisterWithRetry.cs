// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 读取一个 Double 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>double 值</returns>
    public async Task<double> ReadDoubleHoldingRegisterAsync(
        ushort startAddress,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master => master.ReadDoubleHoldingRegisterAsync(_slaveAddress, startAddress, _endian),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 读取 Double 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfDoubleHoldingRegisters">double 类型保持寄存器读取个数</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>double 数组</returns>
    public async Task<double[]> ReadDoubleHoldingRegistersAsync(
        ushort startAddress,
        ushort numberOfDoubleHoldingRegisters,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master =>
                master.ReadDoubleHoldingRegistersAsync(
                    _slaveAddress,
                    startAddress,
                    numberOfDoubleHoldingRegisters,
                    _endian
                ),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
