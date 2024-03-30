namespace Core.HttpLogic;

public record HttpResponse<TResponse> : BaseHttpResponse
{
    /// <summary>
    /// Тело ответа
    /// </summary>
    public required TResponse? Body { get; set; }
}