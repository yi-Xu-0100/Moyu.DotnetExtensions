// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;
// ReSharper disable MemberCanBePrivate.Global

namespace Moyu.ModbusExtensions;
public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 读取 Double 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>double 值</returns>
    public static async Task<double> ReadDoubleHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, 4);
        return registers.ToDouble(endian);
    }

    /// <summary>
    /// 读取 Double 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>double 值</returns>
    public static async Task<double> ReadDoubleHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        return await modbusMaster.ReadDoubleHoldingRegisterAsync(1, startAddress, endian);
    }

    /// <summary>
    /// 读取 Double 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfDoubleHoldingRegisters">double 类型保持寄存器读取个数</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>double 数组</returns>
        public static async Task<double[]> ReadDoubleHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort numberOfDoubleHoldingRegisters,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        if (numberOfDoubleHoldingRegisters > ushort.MaxValue / 4)
            throw new ArgumentOutOfRangeException(
                nameof(numberOfDoubleHoldingRegisters),
                "数量超过 Modbus 协议允许的最大值"
            );

        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(
            slaveAddress,
            startAddress,
            (ushort)(numberOfDoubleHoldingRegisters * 4)
        );
        return registers.ToDoubles(endian);
    }

    /// <summary>
    /// 读取 Double 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfDoubleHoldingRegisters">double 类型保持寄存器读取个数</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>double 数组</returns>
    public static async Task<double[]> ReadDoubleHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfDoubleHoldingRegisters,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.ReadDoubleHoldingRegistersAsync(1, startAddress, numberOfDoubleHoldingRegisters, endian);
}
