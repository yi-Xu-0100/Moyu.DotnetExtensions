// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions;

/// <summary>
/// Modbus帮助类
/// </summary>
public static partial class ModbusHelper
{
    /// <summary>
    /// 重新排列Modbus字节序
    /// </summary>
    /// <param name="raw">字节数组</param>
    /// <param name="endian">字节序</param>
    /// <returns>重新排列后的字节数组</returns>
        public static byte[] ReorderBytes(byte[] raw, ModbusEndian endian) => endian switch
    {
        ModbusEndian.ABCD => raw, // 直接用
        ModbusEndian.BADC => [raw[1], raw[0], raw[3], raw[2], raw[5], raw[4], raw[7], raw[6]],
        ModbusEndian.CDAB => [raw[2], raw[3], raw[0], raw[1], raw[6], raw[7], raw[4], raw[5]],
        ModbusEndian.DCBA => [raw[3], raw[2], raw[1], raw[0], raw[7], raw[6], raw[5], raw[4]],
        _ => raw
    };
}

/// <summary>
/// Modbus字节序
/// </summary>
public enum ModbusEndian
{
    /// <summary>
    /// 标准顺序
    /// </summary>
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    ABCD,

    /// <summary>
    /// 字节交换
    /// </summary>
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    BADC,

    /// <summary>
    /// 寄存器交换
    /// </summary>
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    CDAB,

    /// <summary>
    /// 寄存器+字节交换
    /// </summary>
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    DCBA
}
