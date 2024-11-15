using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils
{
    public class ErrorHandler : IErrorHandler
    {
        private const string ErrorCode = "req_process_failed";
        private const string ErrorMessage = "Unable to process request";
        private const string ErrorDetails = "Error occurred while processing request.";
        private const string ErrorTarget = "generic_exception";


        public ObjectResult HandleError(HttpRequestInfo request, FunctionTimerException error, string errorSource, ILogger log)
        {
            this.LogContextError(log, error);

            ObjectResult errorResponse = null;

            if (error.InnerException is HanaRequestException hanaRequestException)
            {
                request = hanaRequestException.HttpRequestInfo;

                if (hanaRequestException.StatusCode == HttpStatusCode.BadRequest)
                {

                    errorResponse = new BadRequestObjectResult(
                       new ErrorInfo
                       {
                           Errors = JObject.Parse(hanaRequestException.Errors),
                           ResponseInfo = new ResponseMetadata { ResponseTime = $"{error.ExecutionTime.TotalMilliseconds}ms" }
                       });
                }
                else
                {

                    CustomError customError = new CustomError
                    {
                        Code = ErrorCode,
                        Message = ErrorMessage,
                        Details = ErrorDetails,
                        Target = ErrorTarget
                    };

                    errorResponse = new BadRequestObjectResult(
                        new ErrorInfo
                        {
                            Errors = JObject.Parse(JsonConvert.SerializeObject(customError)),
                            ResponseInfo = new ResponseMetadata { ResponseTime = $"{error.ExecutionTime.TotalMilliseconds}ms" }
                        });
                }
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

        public void LogContextInfo(ILogger log, string message)
        {
            log.LogInformation(message);
        }
        public void LogContextError(ILogger log, FunctionTimerException error)
        {
            log.LogError($"Error: {error.InnerException} {error.StackTrace}");
        }
    }
}
