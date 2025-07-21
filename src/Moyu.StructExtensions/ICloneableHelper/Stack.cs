using Moyu.StructExtensions.Interface;

namespace Moyu.StructExtensions;

public static partial class CloneableHelper
{
    /// <summary>
    /// 创建当前 <see cref="Stack{T}"/> 的深拷贝副本。
    /// </summary>
    /// <typeparam name="T">栈元素的类型，必须实现 <see cref="ICloneable{T}"/>。</typeparam>
    /// <param name="stack">要克隆的原始栈。</param>
    /// <returns>一个新的 <see cref="Stack{T}"/> 实例，元素出栈顺序与原始栈一致。</returns>
    /// <exception cref="ArgumentNullException">如果 <paramref name="stack"/> 为 null。</exception>
    /// <remarks>
    /// 为保持元素出栈顺序一致，先反转栈中元素后再进行克隆。
    /// </remarks>
    public static Stack<T> Clone<T>(this Stack<T> stack)
        where T : ICloneable<T> =>
        stack == null ? throw new ArgumentNullException(nameof(stack)) : new Stack<T>(DeepCloneCore(stack.Reverse()));
}
