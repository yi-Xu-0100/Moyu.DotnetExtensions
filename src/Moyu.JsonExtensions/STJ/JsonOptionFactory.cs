using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Moyu.JsonExtensions.STJ;

/// <summary>
/// 提供根据 JsonOptionType 创建并缓存 JsonSerializerOptions 的工厂方法。
/// </summary>
internal static class JsonOptionFactory
{
    private static readonly ConcurrentDictionary<JsonOptionType, JsonSerializerOptions> _cache = new();

    /// <summary>
    /// 获取指定类型的 JsonSerializerOptions。如果已存在则直接返回缓存实例。
    /// </summary>
    /// <param name="type">Json 配置类型，默认 IndentEnc。</param>
    /// <returns>JsonSerializerOptions 实例。</returns>
    internal static JsonSerializerOptions Create(JsonOptionType type = JsonOptionType.IndentEnc)
    {
        return _cache.GetOrAdd(type, CreateOptionsInternal);
    }

    /// <summary>
    /// 内部方法：根据类型创建新的 JsonSerializerOptions。
    /// </summary>
    private static JsonSerializerOptions CreateOptionsInternal(JsonOptionType type)
    {
        JsonSerializerOptions options = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        switch (type)
        {
            case JsonOptionType.IndentEnc:
                options.WriteIndented = true;
                break;

            case JsonOptionType.EncOnly:
                break;

            case JsonOptionType.EncFields:
                options.IncludeFields = true;
                break;

            case JsonOptionType.IndentEncEnumStr:
                options.WriteIndented = true;
                options.Converters.Add(new JsonStringEnumConverter());
                break;

            case JsonOptionType.EncEnumStrFields:
                options.IncludeFields = true;
                options.Converters.Add(new JsonStringEnumConverter());
                break;

            case JsonOptionType.IndentEncEnumStrFields:
                options.IncludeFields = true;
                options.WriteIndented = true;
                options.Converters.Add(new JsonStringEnumConverter());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "不支持的 JsonOptionType。");
        }

        return options;
    }
}
