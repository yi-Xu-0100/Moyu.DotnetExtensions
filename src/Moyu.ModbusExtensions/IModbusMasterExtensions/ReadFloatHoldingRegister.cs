// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 读取 Float 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>float 值</returns>
    public static async Task<float> ReadFloatHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, 2);
        return registers.ToFloat(endian);
    }

    /// <summary>
    /// 读取 Float 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>float 值</returns>
    public static async Task<float> ReadFloatHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        return await modbusMaster.ReadFloatHoldingRegisterAsync(1, startAddress, endian);
    }

    /// <summary>
    /// 读取 Float 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfFloatHoldingRegisters">float 类型保持寄存器读取个数</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>float 数组</returns>
    public static async Task<float[]> ReadFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort numberOfFloatHoldingRegisters,
        ModbusEndian endian = ModbusEndian.ABCD
    )
    {
        if (numberOfFloatHoldingRegisters > ushort.MaxValue / 2)
            throw new ArgumentOutOfRangeException(
                nameof(numberOfFloatHoldingRegisters),
                "数量超过 Modbus 协议允许的最大值"
            );

        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(
            slaveAddress,
            startAddress,
            (ushort)(numberOfFloatHoldingRegisters * 2)
        );
        return registers.ToFloats(endian);
    }

    /// <summary>
    /// 读取 Float 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfFloatHoldingRegisters">float 类型保持寄存器读取个数</param>
    /// <param name="endian">Modbus字节序</param>
    /// <returns>float 数组</returns>
    public static async Task<float[]> ReadFloatHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfFloatHoldingRegisters,
        ModbusEndian endian = ModbusEndian.ABCD
    ) => await modbusMaster.ReadFloatHoldingRegistersAsync(1, startAddress, numberOfFloatHoldingRegisters, endian);
}
