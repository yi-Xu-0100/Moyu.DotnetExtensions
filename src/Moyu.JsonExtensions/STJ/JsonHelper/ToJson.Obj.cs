using System.Text.Json;

namespace Moyu.JsonExtensions.STJ;

/// <summary>
/// 提供 JSON 序列化和反序列化的扩展方法。
/// </summary>
public static partial class JsonHelper
{
    /// <summary>
    /// 将对象序列化为 JSON 字符串，使用指定的 <see cref="JsonSerializerOptions" />。
    /// </summary>
    /// <param name="obj">要序列化的对象。</param>
    /// <param name="options">JSON 序列化选项。</param>
    /// <returns>序列化后的 JSON 字符串。</returns>
    public static string ToJson<T>(T obj, JsonSerializerOptions options)
        where T : class =>
        JsonSerializer.Serialize(obj, options);

    /// <summary>
    /// 将对象序列化为 JSON 字符串，使用预设的 <see cref="JsonOptionType" /> 枚举选项。
    /// </summary>
    /// <param name="obj">要序列化的对象。</param>
    /// <param name="optionType">预设的 JSON 选项类型，默认为 EncEnumStrFields。</param>
    /// <returns>序列化后的 JSON 字符串。</returns>
    public static string ToJson<T>(T obj, JsonOptionType optionType = JsonOptionType.EncEnumStrFields)
        where T : class
    {
        JsonSerializerOptions options = JsonOptionFactory.Create(optionType);
        return JsonSerializer.Serialize(obj, options);
    }
}
