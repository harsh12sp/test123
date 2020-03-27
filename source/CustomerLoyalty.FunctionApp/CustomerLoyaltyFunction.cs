using BenjaminMoore.Api.Retail.Pos.Common.Diagnostics;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp
{
    public class CustomerLoyaltyFunction
    {
        private readonly ICustomerLoyaltyService _customerLoyaltyService;

        public CustomerLoyaltyFunction(ICustomerLoyaltyService customerLoyaltyService)
        {
            _customerLoyaltyService =
                customerLoyaltyService ?? throw new ArgumentNullException(nameof(customerLoyaltyService));
        }

        [FunctionName("CreateCustomerLoyalty")]
        public async Task<IActionResult> CreateCustomerLoyalty(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customerloyalty")] HttpRequest req,
            ILogger log)
        {
            try
            {
                TimedFunctionResult<CustomerLoyaltyIndicator> timedFunctionResult = await FunctionTimer.ExecuteWithTimer(async () =>
                {
                    Customer customer = null;

                    using (StreamReader reader = new StreamReader(req.Body))
                    {
                        string body = await reader.ReadToEndAsync();
                        if (string.IsNullOrWhiteSpace(body))
                        {
                            throw new IOException("Missing of empty request body");
                        }

                        customer = JsonConvert.DeserializeObject<Customer>(body);
                    }

                    return await _customerLoyaltyService.CreateCustomerLoyalty(customer);
                });

                CustomerInfo customerInfo = new CustomerInfo
                {
                    CustomerLoyaltyIndicator = timedFunctionResult.Result,
                    ResponseInfo = new ResponseMetadata {ResponseTime = $"{timedFunctionResult.ExecutionTime.TotalMilliseconds}ms"}

                };

                return new OkObjectResult(customerInfo);
            }
            catch (FunctionTimerException ex) when (ex.InnerException is HanaRequestException hanaRequestException)
            {
                return new BadRequestObjectResult(
                    new ErrorInfo
                    {
                        Errors = JObject.Parse(hanaRequestException.Errors),
                        ResponseInfo = new ResponseMetadata { ResponseTime = $"{ex.ExecutionTime.TotalMilliseconds}ms" }
                    });
            }
            catch (FunctionTimerException ex) when (ex.InnerException is IOException ioException)
            {
                return new BadRequestObjectResult(ioException.Message);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Unable to process request");
            }
        }
    }
}