// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License.See LICENSE for details.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace Moyu.JsonExtensions.STJ;

public static partial class JsonHelper
{
    /// <summary>
    /// 将 <see cref="JsonNode" /> 序列化为 JSON 字符串，使用指定的 <see cref="JsonSerializerOptions" />。
    /// </summary>
    /// <param name="node">要序列化的 <see cref="JsonNode" /> 对象。</param>
    /// <param name="options">用于控制序列化行为的 JSON 配置选项。</param>
    /// <returns>序列化后的 JSON 字符串。</returns>
    public static string ToJson(this JsonNode node, JsonSerializerOptions options) => node.ToJsonString(options);

    /// <summary>
    /// 将 <see cref="JsonNode" /> 序列化为 JSON 字符串，使用预设的 <see cref="JsonOptionType" /> 枚举选项。
    /// </summary>
    /// <param name="node">要序列化的 <see cref="JsonNode" /> 对象。</param>
    /// <param name="optionType">预设的 JSON 配置选项类型，默认为 <see cref="JsonOptionType.EncEnumStrFields" />。</param>
    /// <returns>序列化后的 JSON 字符串。</returns>
    public static string ToJson(this JsonNode node, JsonOptionType optionType = JsonOptionType.EncEnumStrFields)
    {
        JsonSerializerOptions options = JsonOptionFactory.Create(optionType);
        return node.ToJsonString(options);
    }
}
