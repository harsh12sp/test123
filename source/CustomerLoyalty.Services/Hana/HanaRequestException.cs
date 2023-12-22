using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using System;
using System.Net;
using System.Net.Http;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpRequestInfo HttpRequestInfo { get; }
        public string Errors { get; }
        public string HanaRequestPayload { get; }

        public HanaRequestException(string errorMessage, string errors, HttpStatusCode statusCode, HttpRequestInfo httpRequestInfo, string hanaRequestPayload = "") : base(errorMessage)
        {
            StatusCode = statusCode;
            HttpRequestInfo = httpRequestInfo;
            Errors = errors;
            HanaRequestPayload = hanaRequestPayload;
        }
    }
}
