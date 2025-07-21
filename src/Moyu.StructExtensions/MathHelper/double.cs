// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.StructExtensions;

public static partial class MathHelper
{
    /// <summary>
    /// 判断两个 <see cref="double"/> 值是否在指定误差范围内相等。
    /// </summary>
    /// <param name="a">第一个双精度浮点数。</param>
    /// <param name="b">第二个双精度浮点数。</param>
    /// <param name="epsilon">允许的误差范围，默认为 1e-6。</param>
    /// <returns>若两数差的绝对值小于 <paramref name="epsilon"/>，则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    public static bool IsEqual(this double a, double b, double epsilon) => Math.Abs(a - b) < epsilon;

    /// <summary>
    /// 返回指定 <see cref="double"/> 值的符号，考虑给定的误差范围。
    /// </summary>
    /// <param name="value">要判断符号的值。</param>
    /// <param name="epsilon">用于判断是否为零的误差范围。</param>
    /// <returns>若值接近于零（在误差范围内）则返回 <c>0</c>，否则返回 <c>1</c>（正数）或 <c>-1</c>（负数）。</returns>
    public static int Sign(this double value, double epsilon)
    {
        if (Math.Abs(value) < epsilon)
        {
            return 0;
        }

        return value > 0 ? 1 : -1;
    }
}
