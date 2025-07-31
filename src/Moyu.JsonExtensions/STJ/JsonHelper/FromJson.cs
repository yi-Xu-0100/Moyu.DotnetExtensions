// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Text.Json;

namespace Moyu.JsonExtensions.STJ;

public static partial class JsonHelper
{
    /// <summary>
    /// 从 JSON 字符串反序列化为对象，使用指定的 <see cref="JsonSerializerOptions" />。
    /// </summary>
    /// <typeparam name="T">目标对象的类型。</typeparam>
    /// <param name="json">JSON 字符串。</param>
    /// <param name="options">JSON 反序列化选项。</param>
    /// <returns>反序列化后的对象实例。</returns>
    public static T? FromJson<T>(string json, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<T>(json, options);

    /// <summary>
    /// 从 JSON 字符串反序列化为对象，使用预设的 <see cref="JsonOptionType" /> 枚举选项。
    /// </summary>
    /// <typeparam name="T">目标对象的类型。</typeparam>
    /// <param name="json">JSON 字符串。</param>
    /// <param name="optionType">预设的 JSON 选项类型，默认为 EncEnumStrFields。</param>
    /// <returns>反序列化后的对象实例。</returns>
    public static T? FromJson<T>(string json, JsonOptionType optionType = JsonOptionType.EncEnumStrFields)
    {
        JsonSerializerOptions options = JsonOptionFactory.Create(optionType);
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
