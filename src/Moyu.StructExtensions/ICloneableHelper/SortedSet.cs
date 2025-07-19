// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="SortedSet{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">集合元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="set">要克隆的原始排序集合。</param>
    /// <returns>一个新的 <see cref="SortedSet{T}"/> 实例，包含所有元素的深拷贝，且排序规则保持不变。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="set"/> 为 null。</exception>
    /// <remarks>
    /// 克隆后的集合保留原有的排序比较器。
    /// </remarks>
    public static SortedSet<T> Clone<T>(this SortedSet<T> set)
        where T : ICloneable<T> =>
        set == null ? throw new ArgumentNullException(nameof(set)) : new SortedSet<T>(DeepCloneCore(set), set.Comparer);
}
