// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using NModbus;

namespace Moyu.ModbusExtensions;

/// <summary>
/// IModbusMaster 扩展方法
/// </summary>
public static partial class ModbusMasterExtensions
{
    /// <summary>
    /// 读取线圈状态 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfPoints">读取数目</param>
    /// <returns>线圈状态数组</returns>
    public static async Task<bool[]> ReadCoilsAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfPoints
    )
    {
        return await modbusMaster.ReadCoilsAsync(1, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取离散输入状态 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfPoints">读取数目</param>
    /// <returns>离散输入状态数组</returns>
    public static async Task<bool[]> ReadInputsAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfPoints
    )
    {
        return await modbusMaster.ReadInputsAsync(1, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfPoints">读取数目</param>
    /// <returns>保持寄存器数组</returns>
    public static async Task<ushort[]> ReadHoldingRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfPoints
    )
    {
        return await modbusMaster.ReadHoldingRegistersAsync(1, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取输入寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfPoints">读取数目</param>
    /// <returns>输入寄存器数组</returns>
    public static async Task<ushort[]> ReadInputRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort numberOfPoints
    )
    {
        return await modbusMaster.ReadInputRegistersAsync(1, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 写入单个线圈 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">写入值</param>
    /// <returns></returns>
    public static async Task WriteSingleCoilAsync(this IModbusMaster modbusMaster, ushort startAddress, bool value)
    {
        await modbusMaster.WriteSingleCoilAsync(1, startAddress, value);
    }

    /// <summary>
    /// 写入单个保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="value">写入值</param>
    /// <returns></returns>
    public static async Task WriteSingleRegisterAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort value
    )
    {
        await modbusMaster.WriteSingleRegisterAsync(1, startAddress, value);
    }

    /// <summary>
    /// 写入多个保持寄存器 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="data">写入数据</param>
    /// <returns></returns>
    public static async Task WriteMultipleRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startAddress,
        ushort[] data
    )
    {
        await modbusMaster.WriteMultipleRegistersAsync(1, startAddress, data);
    }

    /// <summary>
    /// 写入多个线圈 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="data">写入数据</param>
    /// <returns></returns>
    public static async Task WriteMultipleCoilsAsync(this IModbusMaster modbusMaster, ushort startAddress, bool[] data)
    {
        await modbusMaster.WriteMultipleCoilsAsync(1, startAddress, data);
    }

    /// <summary>
    /// 写入多个保持寄存器后读取 (从机地址 = 1)
    /// </summary>
    /// <param name="modbusMaster">实现接口 IModbusMaster</param>
    /// <param name="startReadAddress">读取起始地址</param>
    /// <param name="numberOfPointsToRead">读取数目</param>
    /// <param name="startWriteAddress">写入起始地址</param>
    /// <param name="writeData">写入数据</param>
    /// <returns>写入后的读取的保持寄存器数组</returns>
    public static async Task<ushort[]> ReadWriteMultipleRegistersAsync(
        this IModbusMaster modbusMaster,
        ushort startReadAddress,
        ushort numberOfPointsToRead,
        ushort startWriteAddress,
        ushort[] writeData
    )
    {
        return await modbusMaster.ReadWriteMultipleRegistersAsync(
            1,
            startReadAddress,
            numberOfPointsToRead,
            startWriteAddress,
            writeData
        );
    }
}
