using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.Common.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<HttpRequestInfo> GetHttpRequestInfo(this HttpRequest request)
        {
            string body;

            using (StreamReader reader = new StreamReader(request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            var httpRequestInfoDto = new HttpRequestInfo
            {
                HttpMethod = request.Method,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString,
                Query = request.Query,
                Headers = new Dictionary<string, string>(),
                Body = body
            };

            return httpRequestInfoDto;
        }

        public static HttpRequestInfo GetRequestInfoFromHttpClient(this HttpResponseMessage httpResponseMessage, string requestPayload)
        {

            HttpRequestInfo httpRequestInfoDto = new HttpRequestInfo
            {
                HttpMethod = httpResponseMessage.RequestMessage?.Method?.ToString() ?? "",
                Host = new HostString(
                    httpResponseMessage.RequestMessage?.RequestUri?.Host?.ToString() ?? "",
                    httpResponseMessage.RequestMessage?.RequestUri?.Port ?? 80),
                Path = httpResponseMessage.RequestMessage?.RequestUri?.LocalPath ?? "",
                QueryString = new QueryString(httpResponseMessage.RequestMessage?.RequestUri?.Query),
                Body = requestPayload ?? ""
            };

            httpRequestInfoDto.Query = QueryHelpers.ParseQuery(httpRequestInfoDto.QueryString.Value);

            return httpRequestInfoDto;
        }
    }
}
