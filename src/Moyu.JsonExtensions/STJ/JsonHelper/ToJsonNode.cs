// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace Moyu.JsonExtensions.STJ;

public static partial class JsonHelper
{
    /// <summary>
    /// 将对象序列化为 <see cref="JsonNode" />，使用指定的 <see cref="JsonSerializerOptions" />。
    /// </summary>
    /// <param name="obj">要序列化的对象。</param>
    /// <param name="options">JSON 序列化选项。</param>
    /// <returns>表示 JSON 数据结构的 <see cref="JsonNode" />，如果对象为 null，则返回 null。</returns>
    public static JsonNode? ToJsonNode(object obj, JsonSerializerOptions options) =>
        JsonSerializer.SerializeToNode(obj, options);

    /// <summary>
    /// 将对象序列化为 <see cref="JsonNode" />，使用预设的 <see cref="JsonOptionType" /> 枚举选项。
    /// </summary>
    /// <param name="obj">要序列化的对象。</param>
    /// <param name="optionType">预设的 JSON 选项类型，默认为 <see cref="JsonOptionType.EncEnumStrFields" />。</param>
    /// <returns>表示 JSON 数据结构的 <see cref="JsonNode" />，如果对象为 null，则返回 null。</returns>
    public static JsonNode? ToJsonNode(object obj, JsonOptionType optionType = JsonOptionType.EncEnumStrFields)
    {
        JsonSerializerOptions options = JsonOptionFactory.Create(optionType);
        return JsonSerializer.SerializeToNode(obj, options);
    }
}
