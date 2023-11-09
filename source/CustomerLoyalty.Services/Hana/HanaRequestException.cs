using System;
using System.Net.Http;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaRequestException : Exception
    {
        public string Errors { get; }
        public HttpResponseMessage ErrorResponseMessage { get; }
        public string HanaRequestPayload { get; }

        public HanaRequestException(string errors, HttpResponseMessage httpResponseMessage, string hanaRequestPayload = "") : base(errors)
        {
            Errors = errors;
            ErrorResponseMessage = httpResponseMessage;
            HanaRequestPayload = hanaRequestPayload;
        }
    }
}
