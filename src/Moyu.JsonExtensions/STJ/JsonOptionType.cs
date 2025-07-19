namespace Moyu.JsonExtensions.STJ;

/// <summary>
/// 表示常用的 JsonSerializerOptions 配置类型。
/// </summary>
public enum JsonOptionType
{
    /// <summary>
    /// 启用缩进格式化输出 + Web 安全编码器（避免 HTML 注入风险）。
    /// </summary>
    IndentEnc,

    /// <summary>
    /// 仅启用 Web 安全编码器，不进行缩进格式化。
    /// </summary>
    EncOnly,

    /// <summary>
    /// 启用编码器，并序列化字段（IncludeFields = true）。
    /// </summary>
    EncFields,

    /// <summary>
    /// 启用缩进格式化输出 + Web 安全编码器（避免 HTML 注入风险），并支持 Enum 到 string。
    /// </summary>
    IndentEncEnumStr,

    /// <summary>
    /// 启用缩进格式化输出 + Web 安全编码器（避免 HTML 注入风险），
    /// 支持 Enum 转字符串，并序列化字段（IncludeFields = true）。
    /// </summary>
    EncEnumStrFields,

    /// <summary>
    /// 启用缩进格式化输出 + Web 安全编码器（避免 HTML 注入风险），
    /// 支持 Enum 转字符串，并序列化字段（IncludeFields = true）。
    /// </summary>
    IndentEncEnumStrFields
}
