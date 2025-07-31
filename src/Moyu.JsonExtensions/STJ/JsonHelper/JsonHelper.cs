// Copyright (c) 2025-now yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

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
    private static JsonSerializerOptions GetOptions(JsonOptionType optionType) =>
        JsonOptionFactory.Create(optionType);

    /// <summary>
    /// 获取启用缩进格式化输出 + Web 安全编码器(避免 HTML 注入风险)的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions IndentEncOptions =>
        GetOptions(JsonOptionType.IndentEnc);

    /// <summary>
    /// 获取启用缩进格式化输出 + Web 安全编码器(避免 HTML 注入风险)的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions EncOnlyOptions =>
        GetOptions(JsonOptionType.EncOnly);

    /// <summary>
    /// 获取启用编码器，并序列化字段(IncludeFields = true)的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions EncFieldsOptions =>
        GetOptions(JsonOptionType.EncFields);

    /// <summary>
    /// 获取启用缩进格式化输出 + Web 安全编码器(避免 HTML 注入风险)，
    /// 支持 Enum 转字符串的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions IndentEncEnumStrOptions =>
        GetOptions(JsonOptionType.IndentEncEnumStr);

    /// <summary>
    /// 获取启用 Web 安全编码器(避免 HTML 注入风险)，
    /// 支持 Enum 转字符串，并序列化字段(IncludeFields = true)的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions EncEnumStrFieldsOptions =>
        GetOptions(JsonOptionType.EncEnumStrFields);

    /// <summary>
    /// 获取启用缩进格式化输出 + Web 安全编码器(避免 HTML 注入风险)，
    /// 支持 Enum 转字符串，并序列化字段(IncludeFields = true)的 JSON 配置选项。
    /// </summary>
    public static JsonSerializerOptions IndentEncEnumStrFieldsOptions =>
        GetOptions(JsonOptionType.IndentEncEnumStrFields);
}
