// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Services;

public partial class ModbusService
{
    /// <summary>
    /// 读取一个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>bool 值</returns>
    public async Task<bool> ReadSignalHoldingRegisterAsync(
        ushort startAddress,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master => master.ReadSignalHoldingRegisterAsync(_slaveAddress, startAddress),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 读取多个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfSignalHoldingRegisters">bool 类型保持寄存器读取个数</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>bool 数组</returns>
    public async Task<bool[]> ReadSignalHoldingRegistersAsync(
        ushort startAddress,
        ushort numberOfSignalHoldingRegisters,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master =>
                master.ReadSignalHoldingRegistersAsync(_slaveAddress, startAddress, numberOfSignalHoldingRegisters),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 读取一个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16)</returns>
    public async Task<bool[]> ReadBitSignalsHoldingRegisterAsync(
        ushort startAddress,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master => master.ReadBitSignalsHoldingRegisterAsync(_slaveAddress, startAddress),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }

    /// <summary>
    /// 读取多个 bool 类型保持寄存器
    /// </summary>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfBitSignalHoldingRegisters">bool 类型保持寄存器读取个数</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="reTryTimeSpan">重试间隔</param>
    /// <param name="waitTimeout">等待超时时间</param>
    /// <param name="token">取消令牌</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16*numberOfBitSignalHoldingRegisters)</returns>
    public async Task<bool[]> ReadBitSignalsHoldingRegistersAsync(
        ushort startAddress,
        ushort numberOfBitSignalHoldingRegisters,
        int maxRetries = 3,
        TimeSpan? reTryTimeSpan = null,
        TimeSpan? waitTimeout = null,
        CancellationToken? token = null
    )
    {
        return await ExecuteRequestAsync(
            master =>
                master.ReadBitSignalsHoldingRegistersAsync(
                    _slaveAddress,
                    startAddress,
                    numberOfBitSignalHoldingRegisters
                ),
            maxRetries,
            reTryTimeSpan,
            waitTimeout,
            token
        );
    }
}
