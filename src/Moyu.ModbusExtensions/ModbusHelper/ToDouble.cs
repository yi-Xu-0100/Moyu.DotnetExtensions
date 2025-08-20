// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions;

public static partial class ModbusHelper
{
    /// <summary>
    /// 将 Modbus 寄存器数组转换为 double 类型
    /// </summary>
    /// <param name="registers">寄存器值（长度必须为 4）</param>
    /// <param name="endian">字节序</param>
    /// <returns>转换后的 double 值</returns>
    /// <exception cref="ArgumentException">如果寄存器数组长度不为 4，则不可转换</exception>
        public static double ToDouble(this ushort[] registers, ModbusEndian endian = ModbusEndian.ABCD)
    {
        if (registers.Length != 4)
        {
            throw new ArgumentException($"{nameof(registers)} 必须是长度为 4 的数组");
        }

        byte[] bytes = new byte[8];
        Buffer.BlockCopy(registers, 0, bytes, 0, 8);

        // 调整字节序
        bytes = ReorderBytes(bytes, endian);

        return BitConverter.ToDouble(bytes, 0);
    }

    /// <summary>
    /// 将 Modbus 寄存器数组批量转换为 double 数组
    /// </summary>
    /// <param name="registers">寄存器数组（长度必须是 4 的倍数）</param>
    /// <param name="endian">字节序</param>
    /// <returns>转换后的 double 数组</returns>
    /// <exception cref="ArgumentException">如果寄存器数组长度不是 4 的倍数，则不可转换</exception>
    public static double[] ToDoubles(this ushort[] registers, ModbusEndian endian = ModbusEndian.ABCD)
    {
        if (registers.Length % 4 != 0)
        {
            throw new ArgumentException($"{nameof(registers)}的长度必须是 4 的倍数");
        }

        int count = registers.Length / 4;
        double[] result = new double[count];

        for (int i = 0; i < count; i++)
        {
            ushort[] slice = new ushort[4];
            Array.Copy(registers, i * 4, slice, 0, 4);
            result[i] = slice.ToDouble(endian);
        }

        return result;
    }
}
