// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="List{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">列表元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="list">要克隆的原始列表。</param>
    /// <returns>一个新的 <see cref="List{T}"/> 实例，其元素为原始列表元素的深拷贝。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="list"/> 为 null。</exception>
    public static List<T> Clone<T>(this List<T> list)
        where T : ICloneable<T> =>
        list == null ? throw new ArgumentNullException(nameof(list)) : DeepCloneCore(list).ToList();
}
