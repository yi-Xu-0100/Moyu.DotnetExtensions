// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 读取 ushort 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>ushort 值</returns>
    public static async Task<ushort> ReadHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress
    )
    {
        ushort[] result = await modbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, 1);
        return result[0];
    }

    /// <summary>
    /// 读取 ushort 类型保持寄存器 (从机地址为 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>ushort 值</returns>
    public static async Task<ushort> ReadHoldingRegisterAsync(this IModbusMaster modbusMaster, ushort startAddress)
    {
        ushort[] result = await modbusMaster.ReadHoldingRegistersAsync(1, startAddress, 1);
        return result[0];
    }
}
