// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="ObservableCollection{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">集合元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="collection">要克隆的原始可观察集合。</param>
    /// <returns>一个新的 <see cref="ObservableCollection{T}"/> 实例，包含所有元素的深拷贝。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="collection"/> 为 null。</exception>
    /// <remarks>
    /// 克隆后的集合包含的元素均为原集合元素的深拷贝。
    /// </remarks>
    public static ObservableCollection<T> Clone<T>(this ObservableCollection<T> collection)
        where T : ICloneable<T> =>
        collection == null
            ? throw new ArgumentNullException(nameof(collection))
            : new ObservableCollection<T>(DeepCloneCore(collection).ToList());
}
