using Core.HttpLogic.Enums;

namespace Core.HttpLogic;

public record HttpRequestData
{
    /// <summary>
    /// Тип метода
    /// </summary>
    public required HttpMethod Method { get; set; }

    /// <summary>
    /// Адрес запроса
    /// </summary>\
    public required string Path { set; get; }

    /// <summary>
    /// Тело метода
    /// </summary>
    public required object? Body { get; set; }

    /// <summary>
    /// content-type, указываемый при запросе
    /// </summary>
    public ContentType ContentType { get; set; } = ContentType.ApplicationJson;

    /// <summary>
    /// Заголовки, передаваемые в запросе
    /// </summary>
    public IDictionary<string, string> HeaderDictionary { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Коллекция параметров запроса
    /// </summary>
    public ICollection<KeyValuePair<string, string?>> QueryParameterList { get; set; } =
        new List<KeyValuePair<string, string?>>();
}