// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 写入一个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    public static async Task WriteHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteSingleRegisterAsync(slaveAddress, startAddress, value);
    }

    /// <summary>
    /// 写入一个 ushort 类型保持寄存器 (从机地址为 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">ushort 值</param>
    /// <param name="endian">Modbus字节序</param>
    public static async Task WriteHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteSingleRegisterAsync(1, startAddress, value);
    }
    /// <summary>
    /// 写入多个 ushort 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    public static async Task WriteHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort[] value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteMultipleRegistersAsync(slaveAddress, startAddress, value);
    }

    /// <summary>
    /// 写入多个 ushort 类型保持寄存器 (从机地址为 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">ushort 值</param>
    /// <param name="endian">Modbus字节序</param>
    public static async Task WriteHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort[] value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteMultipleRegistersAsync(1, startAddress, value);
    }
}
