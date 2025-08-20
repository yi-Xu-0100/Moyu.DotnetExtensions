// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions;

public static partial class ModbusHelper
{
    /// <summary>
    /// 将 double 转换为 Modbus 寄存器数组（4 个 ushort）
    /// </summary>
    /// <param name="value">double 值</param>
    /// <param name="endian">字节序</param>
    /// <returns>寄存器数组（长度为 4）</returns>
    public static ushort[] ToUShorts(this double value, ModbusEndian endian = ModbusEndian.ABCD)
    {
        // double -> byte[8]
        byte[] bytes = BitConverter.GetBytes(value);

        // 调整字节序
        bytes = ReorderBytes(bytes, endian);

        // byte[8] -> ushort[4]
        ushort[] registers = new ushort[4];
        Buffer.BlockCopy(bytes, 0, registers, 0, 8);

        return registers;
    }

    /// <summary>
    /// 将 double 数组批量转换为 Modbus 寄存器数组
    /// </summary>
    /// <param name="values">double 数组</param>
    /// <param name="endian">字节序</param>
    /// <returns>寄存器数组（长度 = values.Length * 4）</returns>
    public static ushort[] ToUShorts(this double[] values, ModbusEndian endian = ModbusEndian.ABCD)
    {
        ushort[] result = new ushort[values.Length * 4];

        for (int i = 0; i < values.Length; i++)
        {
            ushort[] slice = values[i].ToUShorts(endian);
            Array.Copy(slice, 0, result, i * 4, 4);
        }

        return result;
    }

    /// <summary>
    /// 将 bool 值转换为 ushort
    /// </summary>
    /// <param name="value">bool 值</param>
    /// <returns>ushort 值</returns>
    public static ushort ToSignalUShort(this bool value)
    {
        return value ? (ushort)1 : (ushort)0;
    }

    /// <summary>
    /// 将 bool 数组转换为 ushort 数组
    /// </summary>
    /// <param name="values">bool 数组</param>
    /// <returns>ushort 数组</returns>
    public static ushort[] ToSignalUShorts(this bool[] values)
    {
        ushort[] result = new ushort[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i] ? (ushort)1 : (ushort)0;
        }
        return result;
    }

    /// <summary>
    /// 将 16 个 bool 值转换为一个 ushort。
    /// </summary>
    /// <param name="value">
    /// 长度为 16 的 bool 数组，每个元素表示对应位的开关状态。
    /// 数组下标 0 表示最低位（bit0），下标 15 表示最高位（bit15）。
    /// </param>
    /// <returns>
    /// 返回一个 ushort，16 个 bool 按照从低位到高位依次组合。
    /// </returns>
    /// <exception cref="ArgumentException">当 value.Length 不是 16 时抛出</exception>
    /// <example>
    /// 示例:
    /// <code>
    /// bool[] bits = new bool[16] { true, false, true, false, false, false, false, false,
    ///                              false, false, false, false, false, false, false, false };
    /// ushort result = bits.ToBitSignalUShort();
    /// // result == 0b0000_0000_0000_0101 (十进制 5)
    /// </code>
    /// </example>
    public static ushort ToBitSignalUShort(this bool[] value)
    {
        if (value.Length != 16)
            throw new ArgumentException("输入数组长度必须为 16", nameof(value));

        ushort result = 0;

        // 遍历 16 个 bool 值
        for (int i = 0; i < 16; i++)
        {
            if (value[i])
            {
                // 将第 i 位设置为 1
                result |= (ushort)(1 << i);
            }
        }

        return result;
    }

    /// <summary>
    /// 将 bool 数组按每 16 位打包为一个 ushort 数组。
    /// </summary>
    /// <param name="values">
    /// 长度必须是 16 的倍数，每个 bool 表示一个位信号。
    /// 下标低的元素表示低位（bit0），下标高的元素表示高位（bit15）。
    /// </param>
    /// <returns>
    /// 返回一个 ushort 数组，长度 = values.Length / 16。
    /// </returns>
    /// <exception cref="ArgumentException">当 values.Length 不是 16 的倍数时抛出</exception>
    /// <example>
    /// 示例:
    /// <code>
    /// bool[] bits = new bool[32]
    /// {
    ///     // 第 1 个 ushort
    ///     true, false, true, false, false, false, false, false,
    ///     true, false, false, false, false, false, false, false,
    ///
    ///     // 第 2 个 ushort
    ///     true, true, false, false, true, false, false, true,
    ///     false, true, false, true, false, true, false, false
    /// };
    ///
    /// ushort[] result = bits.ToBitSignalUShorts();
    /// // result[0] == 0b0000_0010_0000_0101 (十进制 517)
    /// // result[1] == 0b0101_0100_1001_0011 (十进制 21651)
    /// </code>
    /// </example>
    public static ushort[] ToBitSignalUShorts(this bool[] values)
    {
        if (values.Length % 16 != 0)
            throw new ArgumentException($"{nameof(values)} 的长度必须是 16 的倍数", nameof(values));

        // 计算需要多少个 ushort
        ushort[] result = new ushort[values.Length / 16];

        for (int group = 0; group < result.Length; group++)
        {
            ushort packed = 0;

            // 每组 16 个 bool 压缩为 1 个 ushort
            for (int bit = 0; bit < 16; bit++)
            {
                if (values[group * 16 + bit])
                {
                    packed |= (ushort)(1 << bit);
                }
            }

            result[group] = packed;
        }

        return result;
    }
}
