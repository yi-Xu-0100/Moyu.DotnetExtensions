// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

public static partial class ModbusMasterExtensions
{
    #region 返回 bool ReadSignalHoldingRegister

    /// <summary>
    /// 读取 Boolean 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>Boolean 值</returns>
    public static async Task<bool> ReadSignalHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, 1);
        return registers[0].ToSignal();
    }

    /// <summary>
    /// 读取 Boolean 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>Boolean 值</returns>
    public static async Task<bool> ReadSignalHoldingRegisterAsync(this IModbusMaster modbusMaster, ushort startAddress)
    {
        return await modbusMaster.ReadSignalHoldingRegisterAsync(1, startAddress);
    }

    #endregion

    #region 返回 bool[] ReadSignalHoldingRegisters


    /// <summary>
    /// 读取 boolean 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfSignalHoldingRegisters">boolean 类型保持寄存器读取个数</param>
    /// <returns>boolean 数组</returns>
    public static async Task<bool[]> ReadSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort numberOfSignalHoldingRegisters
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(
            slaveAddress,
            startAddress,
            numberOfSignalHoldingRegisters
        );
        return registers.ToSignals();
    }

    /// <summary>
    /// 读取 boolean 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfSignalHoldingRegisters">boolean 类型保持寄存器读取个数</param>
    /// <returns>boolean 数组</returns>
    public static async Task<bool[]> ReadSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfSignalHoldingRegisters
    ) => await modbusMaster.ReadSignalHoldingRegistersAsync(1, startAddress, numberOfSignalHoldingRegisters);

    #endregion

    #region 返回 bool[] ReadBitSignalsHoldingRegister

    /// <summary>
    /// 读取 boolean 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16)</returns>
    public static async Task<bool[]> ReadBitSignalsHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, 1);
        return registers.ToSignals();
    }

    /// <summary>
    /// 读取 boolean 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16)</returns>
    public static async Task<bool[]> ReadBitSignalsHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress
    ) => await modbusMaster.ReadBitSignalsHoldingRegisterAsync(1, startAddress);

    #endregion

    #region bool[] ReadBitSignalsHoldingRegisters

    /// <summary>
    /// 读取 boolean 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfBitSignalHoldingRegisters">boolean 类型保持寄存器读取个数</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16*numberOfBitSignalHoldingRegisters)</returns>
    public static async Task<bool[]> ReadBitSignalsHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        ushort numberOfBitSignalHoldingRegisters
    )
    {
        ushort[] registers = await modbusMaster.ReadHoldingRegistersAsync(
            slaveAddress,
            startAddress,
            numberOfBitSignalHoldingRegisters
        );
        return registers.ToBitSignals();
    }

    /// <summary>
    /// 读取 boolean 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfBitSignalHoldingRegisters">boolean 类型保持寄存器读取个数</param>
    /// <returns>boolean 数组(对应保持寄存器位, 长度16*numberOfBitSignalHoldingRegisters)</returns>
    public static async Task<bool[]> ReadBitSignalsHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfBitSignalHoldingRegisters
    ) => await modbusMaster.ReadBitSignalsHoldingRegistersAsync(1, startAddress, numberOfBitSignalHoldingRegisters);

    #endregion
}
