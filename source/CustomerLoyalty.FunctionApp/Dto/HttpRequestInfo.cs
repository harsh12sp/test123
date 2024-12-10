using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace BenjaminMoore.Api.Retail.Pos.Common.Dto
{
    public class HttpRequestInfo
    {
        public string HttpMethod { get; set; }
        public HostString Host { get; set; }
        public PathString Path { get; set; }
        public QueryString QueryString { get; set; }
        public IEnumerable<KeyValuePair<string, StringValues>> Query { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
