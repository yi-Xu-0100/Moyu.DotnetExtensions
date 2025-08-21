// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions;

public static partial class ModbusHelper
{
    /// <summary>
    /// 将 Modbus 寄存器数组转换为 float 类型
    /// </summary>
    /// <param name="registers">寄存器值（长度必须为 2）</param>
    /// <param name="endian">字节序</param>
    /// <returns>转换后的 float 值</returns>
    /// <exception cref="ArgumentException">如果寄存器数组长度不为 2，则不可转换</exception>
    public static float ToFloat(this ushort[] registers, ModbusEndian endian = ModbusEndian.ABCD)
    {
        if (registers.Length != 2)
        {
            throw new ArgumentException($"{nameof(registers)} 必须是长度为 2 的数组");
        }

        // ushort[2] -> byte[4]
        byte[] bytes = new byte[4];
        Buffer.BlockCopy(registers, 0, bytes, 0, 4);

        // 调整字节序
        bytes = ReorderBytes(bytes, endian);

        return BitConverter.ToSingle(bytes, 0);
    }

    /// <summary>
    /// 将 Modbus 寄存器数组批量转换为 float 数组
    /// </summary>
    /// <param name="registers">寄存器数组（长度必须是 2 的倍数）</param>
    /// <param name="endian">字节序</param>
    /// <returns>转换后的 float 数组</returns>
    /// <exception cref="ArgumentException">如果寄存器数组长度不是 2 的倍数，则不可转换</exception>
    public static float[] ToFloats(this ushort[] registers, ModbusEndian endian = ModbusEndian.ABCD)
    {
        if (registers.Length % 2 != 0)
        {
            throw new ArgumentException($"{nameof(registers)} 的长度必须是 2 的倍数");
        }

        int count = registers.Length / 2;
        float[] result = new float[count];

        for (int i = 0; i < count; i++)
        {
            ushort[] slice = new ushort[2];
            Array.Copy(registers, i * 2, slice, 0, 2);
            result[i] = slice.ToFloat(endian);
        }

        return result;
    }
}
