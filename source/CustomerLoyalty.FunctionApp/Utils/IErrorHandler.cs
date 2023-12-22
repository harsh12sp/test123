using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils
{
    public interface IErrorHandler
    {
        ObjectResult HandleError(HttpRequestInfo request, FunctionTimerException error, string errorSource, ILogger log);
    }
}
