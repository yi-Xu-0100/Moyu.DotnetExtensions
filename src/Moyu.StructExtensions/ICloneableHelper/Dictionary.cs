// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License.See LICENSE for details.

using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="Dictionary{TKey, TValue}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="TKey">字典键的类型，必须实现 <see cref="ICloneable{TKey}"/>。</typeparam>
    /// <typeparam name="TValue">字典值的类型，必须实现 <see cref="ICloneable{TValue}"/>。</typeparam>
    /// <param name="dictionary">要克隆的原始字典。</param>
    /// <returns>一个新的 <see cref="Dictionary{TKey, TValue}"/> 实例，键和值均为深拷贝。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="dictionary"/> 为 null。</exception>
    /// <remarks>
    /// 克隆后的字典保留原有的比较器行为。
    /// </remarks>
    public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        where TKey : ICloneable<TKey>
        where TValue : ICloneable<TValue> =>
        dictionary == null
            ? throw new ArgumentNullException(nameof(dictionary))
            : dictionary.ToDictionary(kvp => kvp.Key.Clone(), kvp => kvp.Value.Clone(), dictionary.Comparer);
}
