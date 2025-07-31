// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="HashSet{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">集合元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="set">要克隆的原始集合。</param>
    /// <returns>一个新的 <see cref="HashSet{T}"/> 实例，包含所有元素的深拷贝。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="set"/> 为 null。</exception>
    /// <remarks>
    /// 克隆后的集合保留原有的比较器行为。
    /// </remarks>
    public static HashSet<T> Clone<T>(this HashSet<T> set)
        where T : ICloneable<T> =>
        set == null ? throw new ArgumentNullException(nameof(set)) : new HashSet<T>(DeepCloneCore(set), set.Comparer);
}
