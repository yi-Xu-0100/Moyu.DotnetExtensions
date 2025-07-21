// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License.See LICENSE for details.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="Queue{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">队列元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="queue">要克隆的原始队列。</param>
    /// <returns>一个新的 <see cref="Queue{T}"/> 实例，包含所有元素的深拷贝，顺序保持一致。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="queue"/> 为 null。</exception>
    public static Queue<T> Clone<T>(this Queue<T> queue) where T : ICloneable<T>
        => queue == null ? throw new ArgumentNullException(nameof(queue)) : new Queue<T>(DeepCloneCore(queue));
}
