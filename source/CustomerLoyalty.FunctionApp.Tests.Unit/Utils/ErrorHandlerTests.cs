using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using BenjaminMoore.Api.Retail.Pos.Common.Logger;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Unit.Utils
{
    public class ErrorHandlerTests
    {
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<HttpRequestInfo> _requestMock;

        public ErrorHandlerTests()
        {
            _loggerServiceMock = new Mock<ILoggerService>();
            _loggerMock = new Mock<ILogger>();
            _requestMock = new Mock<HttpRequestInfo>();

            _errorHandler = new ErrorHandler(_loggerServiceMock.Object);
        }

        [Fact]
        public void HandleError_HanaRequestException_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var executionTime = TimeSpan.FromSeconds(2);
            var hanaException = new HanaRequestException("Test error message", ErrorCollection.GetSerializedErrors(), HttpStatusCode.BadRequest, new HttpRequestInfo { HttpMethod = "Post" }, "testing payload");
            var error = new FunctionTimerException(executionTime, "Hana error", hanaException);

            // Act
            var result = _errorHandler.HandleError(_requestMock.Object, error, "TestFunction", _loggerMock.Object) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<ErrorInfo>(result.Value);
        }

        [Fact]
        public void HandleError_ArgumentNullException_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var executionTime = TimeSpan.FromSeconds(2);
            var argumentNullException = new ArgumentNullException("paramName", "Argument is null");
            var error = new FunctionTimerException(executionTime, "Missing argument", argumentNullException);

            // Act
            var result = _errorHandler.HandleError(_requestMock.Object, error, "TestFunction", _loggerMock.Object) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<string>(result.Value);
        }

        [Fact]
        public void HandleError_IOException_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var executionTime = TimeSpan.FromSeconds(2);
            var IOException = new IOException("IO error");
            var error = new FunctionTimerException(executionTime, "Io Exception", IOException);

            // Act
            var result = _errorHandler.HandleError(_requestMock.Object, error, "TestFunction", _loggerMock.Object) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<string>(result.Value);
        }

        [Fact]
        public void HandleError_GenericException_ReturnsBadRequestWithCustomError()
        {
            // Arrange
            var executionTime = TimeSpan.FromSeconds(2);
            var genericException = new Exception("Generic exception");
            var error = new FunctionTimerException(executionTime, "Generic Exception", genericException);

            // Act
            var result = _errorHandler.HandleError(_requestMock.Object, error, "TestFunction", _loggerMock.Object) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<ErrorInfo>(result.Value);
        }
    }

    internal class ErrorCollection
    {
        public static string GetSerializedErrors()
        {
            ErrorCollection root = new ErrorCollection(new Error { Code = "Failed", Message = "Missing Email" });

            return JsonConvert.SerializeObject(root);
        }

        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        public ErrorCollection()
        {

        }

        public ErrorCollection(params Error[] errors)
        {
            Errors = errors;
        }
    }

    internal class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
