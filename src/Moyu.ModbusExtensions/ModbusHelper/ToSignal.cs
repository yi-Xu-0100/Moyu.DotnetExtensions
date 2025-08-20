// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions;

public static partial class ModbusHelper
{
    /// <summary>
    /// 将 ushort 值转换为信号量
    /// </summary>
    /// <param name="value">ushort 值</param>
    /// <param name="index">位索引（0-15）</param>
    /// <returns>信号量</returns>
    /// <exception cref="ArgumentOutOfRangeException">如果位索引不在[0,15]范围内, 则引发异常</exception>
    public static bool ToBitSignal(this ushort value, int index = 0)
    {
        if (index is < 0 or >= 16)
            throw new ArgumentOutOfRangeException(nameof(index), "位索引不在[0,15]范围内");

        return (value & (1 << index)) != 0;
    }

    /// <summary>
    /// 将 ushort[] 转换为信号量数组 使用 <see cref="ToBitSignal"/>
    /// </summary>
    /// <param name="value">ushort 值</param>
    /// <returns>信号量数组</returns>
    public static bool[] ToBitSignals(this ushort value)
    {
        bool[] signals = new bool[16];
        for (int i = 0; i < 16; i++)
        {
            // 通过位掩码提取每一位的值（从低位到高位）
            signals[i] = (value & (1 << i)) != 0;
        }
        return signals;
    }

    /// <summary>
    /// 将 ushort[] 转换为信号量数组
    /// </summary>
    /// <param name="values">ushort 数组</param>
    /// <returns>信号量数组</returns>
    public static bool[] ToBitSignals(this ushort[] values)
    {
        bool[] result = new bool[values.Length * 16];

        for (int i = 0; i < values.Length; i++)
        {
            for (int bit = 0; bit < 16; bit++)
            {
                result[i * 16 + bit] = (values[i] & (1 << bit)) != 0;
            }
        }

        return result;
    }

    /// <summary>
    /// 将 ushort 值转换为信号量（value != 0）
    /// </summary>
    /// <param name="value">ushort 值</param>
    /// <returns>信号量</returns>
    public static bool ToSignal(this ushort value)
    {
        return value != 0;
    }

    /// <summary>
    /// 将 ushort[] 转换为 boolean[] 使用 <see cref="ToSignal"/>
    /// </summary>
    /// <param name="value">ushort 数组</param>
    /// <returns>boolean 数组</returns>
    public static bool[] ToSignals(this ushort[] value)
    {
        bool[] result = new bool[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            result[i] = value[i].ToSignal();
        }

        return result;
    }
}
