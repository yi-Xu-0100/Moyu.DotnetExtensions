using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyu.StructExtensions.Interface;

/// <summary>
/// 定义了一个泛型接口，用于创建对象的深拷贝。
/// </summary>
/// <typeparam name="T">要克隆的对象的类型。</typeparam>
/// <remarks>
/// 实现此接口的类必须提供一个克隆方法，该方法创建并返回对象的一个深拷贝。
/// 深拷贝意味着新对象的所有成员（包括引用类型的成员）都是原始对象成员的副本，而不是引用。
/// </remarks>
public interface ICloneable<T>
{
    /// <summary>
    /// 创建并返回当前对象的一个深拷贝。
    /// </summary>
    /// <returns>当前对象的一个深拷贝。</returns>
    /// <exception cref="NotSupportedException">如果克隆操作不支持，则可能抛出此异常。</exception>
    /// <remarks>
    /// 实现此方法的类需要确保返回的对象是一个新的实例，并且其所有成员都是当前对象成员的副本。
    /// 如果对象包含对其他对象的引用，则这些引用也需要被克隆，以确保新对象的独立性。
    /// </remarks>
    public T Clone();
}
