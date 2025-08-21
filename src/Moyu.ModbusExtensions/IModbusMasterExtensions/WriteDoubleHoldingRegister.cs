// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 写入 Double 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">double 数组</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteDoubleHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        double[] values,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        ushort[] data = values.ToUShorts(endian);
        await modbusMaster.WriteMultipleRegistersAsync(slaveAddress, startAddress, data);
    }

    /// <summary>
    /// 写入 Double 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">double 值</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteDoubleHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        double value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteDoubleHoldingRegistersAsync(slaveAddress, startAddress, [value], endian);
    }

    /// <summary>
    /// 写入 Double 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">double 数组</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteDoubleHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        double[] values,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.WriteDoubleHoldingRegistersAsync(1, startAddress, values, endian);

    /// <summary>
    /// 写入 Double 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">double 值</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteDoubleHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        double value,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.WriteDoubleHoldingRegisterAsync(1, startAddress, value, endian);
}
