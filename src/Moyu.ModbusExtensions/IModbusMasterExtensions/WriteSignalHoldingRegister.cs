// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;
public static partial class ModbusMasterExtensions
{
    #region WriteSignalHoldingRegister bool 写入单个寄存器(1 boolean => 1 register)

    /// <summary>
    /// 写入 Boolean 类型保持寄存器
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">boolean 值</param>
    /// <returns></returns>
    public static async Task WriteSignalHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        bool value
    )
    {
        ushort data = value.ToSignalUShort();
        await modbusMaster.WriteSingleRegisterAsync(slaveAddress, startAddress, data);
    }

    /// <summary>
    /// 写入 Boolean 类型保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">Boolean 值</param>
    /// <returns></returns>
    public static async Task WriteBooleanHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        bool value
    ) => await modbusMaster.WriteSignalHoldingRegisterAsync(1, startAddress, value);

    #endregion

    #region WriteSignalHoldingRegisters bool[] 写入多个寄存器(1 boolean => 1 register)

    /// <summary>
    /// 写入 Boolean 类型到多个保持寄存器 (1 boolean => 1 register)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">Boolean 数组</param>
    /// <returns></returns>
    public static async Task WriteSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        bool[] values
    )
    {
        ushort[] data = values.ToSignalUShorts();
        await modbusMaster.WriteMultipleRegistersAsync(slaveAddress, startAddress, data);
    }


    /// <summary>
    /// 写入 Boolean 类型到多个保持寄存器 (1 boolean => 1 register) (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">Boolean 数组</param>
    /// <returns></returns>
    public static async Task WriteSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        bool[] values
    ) => await modbusMaster.WriteSignalHoldingRegistersAsync(1, startAddress, values);

    #endregion

    #region WriteBitSignalHoldingRegister bool[] 写入一个寄存器(16 boolean => 1 register)

    /// <summary>
    /// 写入 Boolean 类型的一个寄存器(16 boolean => 1 register)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">boolean[] 数组</param>
    /// <returns></returns>
    public static async Task WriteBitSignalHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        bool[] values
    )
    {
        ushort data = values.ToBitSignalUShort();
        await modbusMaster.WriteSingleRegisterAsync(slaveAddress, startAddress, data);
    }

    /// <summary>
    /// 写入 Boolean 类型的一个寄存器(16 boolean => 1 register) (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">boolean[] 数组</param>
    /// <returns></returns>
    public static async Task WriteBitBooleanHoldingRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        bool[] values
    ) => await modbusMaster.WriteBitSignalHoldingRegisterAsync(1, startAddress, values);

    #endregion

    #region WriteBitSignalHoldingRegisters bool[] 写入多个寄存器(16 boolean => 1 register)

    /// <summary>
    /// 写入 Boolean 类型到多个保持寄存器 (16 boolean => 1 register)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="slaveAddress">从机地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">Boolean 数组</param>
    /// <returns></returns>
    public static async Task WriteBitSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        byte slaveAddress,
        ushort startAddress,
        bool[] values
    )
    {
        ushort[] data = values.ToBitSignalUShorts();
        await modbusMaster.WriteMultipleRegistersAsync(slaveAddress, startAddress, data);
    }


    /// <summary>
    /// 写入 Boolean 类型到多个保持寄存器 (16 boolean => 1 register) (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="values">Boolean 数组</param>
    /// <returns></returns>
    public static async Task WriteBitSignalHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        bool[] values
    ) => await modbusMaster.WriteBitSignalHoldingRegistersAsync(1, startAddress, values);


    #endregion
}
