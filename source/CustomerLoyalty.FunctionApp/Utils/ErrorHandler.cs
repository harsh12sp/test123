using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using BenjaminMoore.Api.Retail.Pos.Common.Logger;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using BenjaminMoore.Api.Retail.Pos.Common.Extensions;
using System.Net;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils
{
    public class ErrorHandler : IErrorHandler
    {
        private const string ErrorCode = "req_process_failed";
        private const string ErrorMessage = "Unable to process request";
        private const string ErrorDetails = "Error occurred while processing request.";
        private const string ErrorTarget = "generic_exception";
        private readonly ILoggerService _loggerService;

        public ErrorHandler(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public ObjectResult HandleError(HttpRequestInfo request, FunctionTimerException error, string errorSource, ILogger log)
        {
            this.LogContextError(log, error);

            ObjectResult errorResponse = null;

            if (error.InnerException is HanaRequestException hanaRequestException)
            {
                request = hanaRequestException.HttpRequestInfo;

                if (hanaRequestException.StatusCode == HttpStatusCode.BadRequest)
                {
                    _loggerService.LogWarning(hanaRequestException.Message, hanaRequestException.Errors, request, errorSource);

                    errorResponse = new BadRequestObjectResult(
                       new ErrorInfo
                       {
                           Errors = JObject.Parse(hanaRequestException.Errors),
                           ResponseInfo = new ResponseMetadata { ResponseTime = $"{error.ExecutionTime.TotalMilliseconds}ms" }
                       });
                }
                else
                {
                    _loggerService.LogError(hanaRequestException.GetType().Name, (int)hanaRequestException.StatusCode, hanaRequestException.Message,
                   hanaRequestException.Errors, hanaRequestException.StackTrace, hanaRequestException.HttpRequestInfo, errorSource);

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
                _loggerService.LogWarning(argumentNullException.Message, argumentNullException.StackTrace, request, errorSource);
                errorResponse = new BadRequestObjectResult(argumentNullException.Message);
            }
            else if (error.InnerException is IOException ioException)
            {
                _loggerService.LogWarning(ioException.Message, ioException.StackTrace, request, errorSource);
                errorResponse = new BadRequestObjectResult(ioException.Message);
            }
            else if (error.InnerException is Exception exception)
            {
                _loggerService.LogError(exception.GetType().Name, (int)HttpStatusCode.InternalServerError, exception.Message, null, exception.StackTrace, request, errorSource);

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
