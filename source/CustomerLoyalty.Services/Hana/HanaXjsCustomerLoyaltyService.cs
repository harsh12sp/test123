using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using Microsoft.Azure.EventGrid.Models;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaXjsCustomerLoyaltyService : ICustomerLoyaltyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IEventPublisher<Customer> _customerEventPublisher;


        public HanaXjsCustomerLoyaltyService(IHttpClientFactory httpClientFactory, IConfigurationSettings configurationSettings, IEventPublisher<Customer> customerEventPublisher)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configurationSettings =
                configurationSettings ?? throw new ArgumentNullException(nameof(configurationSettings));
            _customerEventPublisher = customerEventPublisher ?? throw new ArgumentNullException(nameof(customerEventPublisher));
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

            await _customerEventPublisher.Publish(c => new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                Subject = c.BenjaminMooreCustomerId,
                Data = c,
                EventType = "customer-loyalty-created",
                EventTime = DateTime.UtcNow,
                DataVersion = "1.5"
            }, customer);

            return new CustomerLoyaltyIndicator
            {
                BmcId = hanaXjsPayload.BmcId,
                LoyaltyIndicator = hanaXjsPayload.LoyaltyIndicator
            };
        }
    }
}