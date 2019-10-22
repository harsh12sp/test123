using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana.Entities;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaXjsCustomerLoyaltyService : ICustomerLoyaltyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationSettings _configurationSettings;

        public HanaXjsCustomerLoyaltyService(IHttpClientFactory httpClientFactory, IConfigurationSettings configurationSettings)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configurationSettings =
                configurationSettings ?? throw new ArgumentNullException(nameof(configurationSettings));
        }

        public async Task<CustomerLoyaltyIndicator> CreateCustomerLoyalty(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            HanaCreateCustomerLoyaltyRequest customerLoyaltyRequest = new HanaCreateCustomerLoyaltyRequest
            {
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                BenjaminMooreCustomerId = customer.BenjaminMooreCustomerId,
                BusinessEmailAddress = customer.BusinessEmailAddress,
                BusinessName = customer.BusinessName,
                BusinessPhoneNumber = customer.BusinessPhoneNumber,
                BusinessType = customer.BusinessType,
                City = customer.City,
                ContactEmailAddress = customer.ContactEmailAddress,
                ContactPhoneNumber = customer.ContactPhoneNumber,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                LoyaltyEmailAddress = customer.LoyaltyEmailAddress,
                Outlet = customer.Outlet,
                PostalCode = customer.PostalCode,
                RetailerId = customer.RetailerId,
                State = customer.State
            };

            HttpClient client = await _httpClientFactory.CreateHttpClient();

            HttpResponseMessage response = await client.PostAsJsonAsync(_configurationSettings.CreateCustomerLoyaltyEndpoint, customerLoyaltyRequest);

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.BadRequest)
            {
                string body = await response.Content.ReadAsStringAsync();
                throw new HanaRequestException(body);
            }

            response.EnsureSuccessStatusCode();

            HanaXjsCreateLoyaltyResponse hanaXjsPayload = await response.Content.ReadAsAsync<HanaXjsCreateLoyaltyResponse>();

            return new CustomerLoyaltyIndicator
            {
                BmcId = hanaXjsPayload.BmcId,
                LoyaltyIndicator = hanaXjsPayload.LoyaltyIndicator
            };
        }
    }
}