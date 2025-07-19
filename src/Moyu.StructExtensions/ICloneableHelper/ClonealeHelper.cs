// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

/// <summary>
/// 提供用于实现 <see cref="ICloneable{T}"/> 接口对象的扩展方法，
/// 便于执行深拷贝操作的辅助工具类。
/// </summary>
/// <remarks>
/// 本类中的扩展方法适用于实现了 <see cref="ICloneable{T}"/> 接口的类型，
/// 可用于集合（如 <see cref="List{T}"/>）的递归深度克隆，
/// 确保克隆对象之间互不影响，适用于不可变性要求较高的业务场景。
/// </remarks>
public static partial class CloneableHelper
{
    /// <summary>
    /// 将实现 <see cref="ICloneable{T}"/> 接口的序列中的所有元素进行深拷贝。
    /// 作为基础逻辑，供其他集合扩展方法调用。
    /// </summary>
    /// <typeparam name="T">元素类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="source">要深拷贝的元素序列。</param>
    /// <returns>包含所有元素克隆副本的 <see cref="IEnumerable{T}"/> 序列。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="source"/> 为 null。</exception>
    private static IEnumerable<T> DeepCloneCore<T>(IEnumerable<T> source) where T : ICloneable<T>
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (T item in source)
        {
            yield return item.Clone();
        }
    }
}
