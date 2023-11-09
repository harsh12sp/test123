using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;

namespace BenjaminMoore.Api.Retail.Pos.Customers.FunctionApp.TestHelpers
{
    public class TestFakeErrorHandler : IErrorHandler
    {
        private const string ErrorCode = "req_process_failed";
        private const string ErrorMessage = "Unable to process request";
        private const string ErrorDetails = "Error occurred while processing request.";
        private const string ErrorTarget = "generic_exception";

        public ObjectResult HandleError(HttpRequestInfo request, FunctionTimerException error, string functionName, ILogger log)
        {
            ObjectResult errorResponse = null;

            if (error.InnerException is HanaRequestException hanaRequestException)
            {
                errorResponse = new BadRequestObjectResult(
                   new ErrorInfo
                   {
                       Errors = JObject.Parse(hanaRequestException.Errors),
                       ResponseInfo = new ResponseMetadata { ResponseTime = $"{error.ExecutionTime.TotalMilliseconds}ms" }
                   });
            }
            else if (error.InnerException is ArgumentNullException argumentNullException)
            {
                errorResponse = new BadRequestObjectResult(argumentNullException.Message);
            }
            else if (error.InnerException is IOException ioException)
            {
                errorResponse = new BadRequestObjectResult(ioException.Message);
            }
            else if (error.InnerException is Exception exception)
            {
                CustomError errors = new CustomError
                {
                    Code = ErrorCode,
                    Message = ErrorMessage,
                    Details = ErrorDetails,
                    Target = ErrorTarget
                };

                errorResponse = new BadRequestObjectResult(
                    new ErrorInfo
                    {
                        Errors = JObject.Parse(JsonConvert.SerializeObject(errors)),
                        ResponseInfo = new ResponseMetadata { ResponseTime = $"{error.ExecutionTime.TotalMilliseconds}ms" }
                    });
            }

            return errorResponse;
        }
    }
}
