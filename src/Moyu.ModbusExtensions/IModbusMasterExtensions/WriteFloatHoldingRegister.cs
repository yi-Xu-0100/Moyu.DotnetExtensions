// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;
public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 写入 Float 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">float 数组</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        float[] values,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        ushort[] data = values.ToUShorts(endian);
        await modbusMaster.WriteMultipleRegistersAsync(slaveAddress, startAddress, data);
    }

    /// <summary>
    /// 写入 Float 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">float 值</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        float value,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        await modbusMaster.WriteFloatHoldingRegistersAsync(slaveAddress, startAddress, [value], endian);
    }

    /// <summary>
    /// 写入 Float 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">float 数组</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        float[] values,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.WriteFloatHoldingRegistersAsync(1, startAddress, values, endian);

    /// <summary>
    /// 写入 Float 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">float 值</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns></returns>
    public static async Task WriteFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        float value,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.WriteFloatHoldingRegistersAsync(1, startAddress, value, endian);
}
