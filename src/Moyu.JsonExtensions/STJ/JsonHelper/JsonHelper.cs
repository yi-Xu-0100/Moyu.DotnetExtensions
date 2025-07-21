// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License.See LICENSE for details.

using System.Text.Json;

namespace Moyu.JsonExtensions.STJ;

/// <summary>
/// 提供 JSON 处理相关的扩展方法。
/// </summary>
public static partial class JsonHelper
{
    /// <summary>
    /// 根据预设的 <see cref="JsonOptionType" /> 获取对应的 <see cref="JsonSerializerOptions" /> 配置。
    /// </summary>
    /// <param name="optionType">JSON 配置类型，默认使用 <see cref="JsonOptionType.EncEnumStrFields" />。</param>
    /// <returns>对应的 JSON 配置选项。</returns>
    public static JsonSerializerOptions GetOptions(JsonOptionType optionType = JsonOptionType.EncEnumStrFields) =>
        JsonOptionFactory.Create(optionType);
}
