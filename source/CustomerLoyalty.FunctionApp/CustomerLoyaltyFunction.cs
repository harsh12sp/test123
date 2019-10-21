using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp
{
    public static class CustomerLoyaltyFunction
    {
        [FunctionName("CreateCustomerLoyalty")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customerloyalty")] HttpRequest req,
            ILogger log)
        {
            return await Task.FromResult(new OkResult());
        }
    }
}
