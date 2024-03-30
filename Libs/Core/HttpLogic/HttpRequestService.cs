using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Core.HttpLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ContentType = Core.HttpLogic.Enums.ContentType;

namespace Core.HttpLogic;

/// <inheritdoc />
internal class HttpRequestService(IHttpConnectionService httpConnectionService) : IHttpRequestService
{
    /// <inheritdoc />
    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, 
        HttpConnectionData connectionData)
    {
        var client = httpConnectionService.CreateHttpClient(connectionData);
        HttpContent? content = null;
        if (requestData.Body is not null) 
            content = PrepareContent(requestData.Body, requestData.ContentType);
        var uri = new Uri(QueryHelpers.AddQueryString(requestData.Path, requestData.QueryParameterList));
        var httpRequestMessage = new HttpRequestMessage(requestData.Method, uri);
        httpRequestMessage.Content = content;
        foreach (var header in requestData.HeaderDictionary)
            httpRequestMessage.Headers.Add(header.Key, header.Value);

        var res = await httpConnectionService.SendRequestAsync(httpRequestMessage, client, default);

        var response = new HttpResponse<TResponse>
        {
            Body = await res.Content.ReadFromJsonAsync<TResponse>(),
            StatusCode = res.StatusCode,
            Headers = res.Headers,
            ContentHeaders = res.Content.Headers
        };
        return response;
    }

    private static HttpContent PrepareContent(object body, ContentType contentType)
    {
        switch (contentType)
        {
            case ContentType.ApplicationJson:
            {
                if (body is string stringBody)
                {
                    body = JToken.Parse(stringBody);
                }

                var serializeSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                return content;
            }

            case ContentType.XWwwFormUrlEncoded:
            {
                if (body is not IEnumerable<KeyValuePair<string, string>> list)
                {
                    throw new Exception(
                        $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                }

                return new FormUrlEncodedContent(list);
            }
            case ContentType.ApplicationXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
            }
            case ContentType.Binary:
            {
                if (body.GetType() != typeof(byte[]))
                {
                    throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                }

                return new ByteArrayContent((byte[])body);
            }
            case ContentType.TextXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
    }
}