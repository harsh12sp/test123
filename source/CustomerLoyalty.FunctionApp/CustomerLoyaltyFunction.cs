using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using BenjaminMoore.Api.Retail.Pos.Common.Extensions;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp
{
    public class CustomerLoyaltyFunction
    {
        private readonly ICustomerLoyaltyService _customerLoyaltyService;
        private readonly IErrorHandler _errorHandler;
        private readonly ILogger<CustomerLoyaltyFunction> _logger;

        public CustomerLoyaltyFunction(ICustomerLoyaltyService customerLoyaltyService, IErrorHandler errorHandler, ILogger<CustomerLoyaltyFunction> logger)
        {
            _customerLoyaltyService =
                customerLoyaltyService ?? throw new ArgumentNullException(nameof(customerLoyaltyService));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Function("CreateCustomerLoyalty")]
        public async Task<IActionResult> CreateCustomerLoyalty(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customerloyalty")] HttpRequest req)
        {
            HttpRequestInfo requestInfo = null;
            try
            {
                TimedFunctionResult<CustomerLoyaltyIndicator> timedFunctionResult = await FunctionTimer.ExecuteWithTimer(async () =>
                {
                    Customer customer = null;
                    requestInfo = await req.GetHttpRequestInfo();

                    if (string.IsNullOrWhiteSpace(requestInfo.Body))
                    {
                        throw new IOException("Missing of empty request body");
                    }

                    customer = JsonConvert.DeserializeObject<Customer>(requestInfo.Body);

                    return await _customerLoyaltyService.CreateCustomerLoyalty(customer);
                });

                CustomerInfo customerInfo = new CustomerInfo
                {
                    CustomerLoyaltyIndicator = timedFunctionResult.Result,
                    ResponseInfo = new ResponseMetadata {ResponseTime = $"{timedFunctionResult.ExecutionTime.TotalMilliseconds}ms"}

                };

                return new OkObjectResult(customerInfo);
            }
            catch (FunctionTimerException ex)
            {
                return _errorHandler.HandleError(requestInfo, ex, Constant.CreateCustomerLoyaltyFunctionName, _logger);
            }
        }
    }
}