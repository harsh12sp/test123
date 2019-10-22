using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Integration
{
    public class CustomerLoyaltyFunctionTests
    {
        [Fact(Skip = "This won't run in azure devops either (unless we have a custom build server)")]
        public async Task GetCustomers_WhenCalledWithInvalidRoute_ShouldReturnA200()
        {
            AzureFunctionsHostSettings azureFunctionsHostSettings = Settings.GetFromAppsettings<AzureFunctionsHostSettings>();

            using (AzureFunctionsHost functionsHost = new AzureFunctionsHost(azureFunctionsHostSettings).Start())
            {
                HttpClient httpClient = functionsHost.GetClient();

                // Act

                HttpResponseMessage response = await httpClient.PostAsJsonAsync("/retailers/pos/customerloyalty",
                    new Customer
                    {
                        Address1 = "123 Test Drive",
                        BenjaminMooreCustomerId = "AG1",
                        Address2 = "Apartment 101",
                        BusinessEmailAddress = "test@test.test",
                        BusinessName = "Test",
                        BusinessPhoneNumber = "5555555555",
                        BusinessType = "paint test",
                        City = "Test",
                        ContactEmailAddress = "test@test.test",
                        ContactPhoneNumber = "5555555555",
                        FirstName = "T",
                        LastName = "Est",
                        LoyaltyEmailAddress = "test@test.test",
                        Outlet = "Test",
                        PostalCode = "12345",
                        RetailerId = "AG1R",
                        State = "NY"
                    });

                // Assert

                Assert.True(response.StatusCode == HttpStatusCode.OK, $"{response.StatusCode} != HttpStatusCode.OK");
            }
        }
    }
}
