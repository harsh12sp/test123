using Azure.Messaging.EventGrid;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
                PostalCode = customer.PostalCode,
                RetailerId = customer.RetailerId,
                State = customer.State,
                LanguageCode = customer.LanguageCode
            };

            if (string.IsNullOrWhiteSpace(customerLoyaltyRequest.LanguageCode))
            {
                customerLoyaltyRequest.LanguageCode = _configurationSettings.DefaultLanguageCode;
            }

            HttpClient client = await _httpClientFactory.CreateHttpClient();
            string createCustomerLoyaltyEndpoint = _configurationSettings.CreateCustomerLoyaltyHanaBaseUrl + "/" +
                                                                                _configurationSettings.CreateCustomerLoyaltyHanaEndPoint;
            HttpResponseMessage response = await client.PostAsJsonAsync(createCustomerLoyaltyEndpoint, customerLoyaltyRequest);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    throw new HanaRequestException(body);
                }
                else
                {
                    throw new Exception();
                }
            }

            response.EnsureSuccessStatusCode();

            HanaXjsCreateLoyaltyResponse hanaXjsPayload = await response.Content.ReadAsAsync<HanaXjsCreateLoyaltyResponse>();

            customer.SegmentCode = hanaXjsPayload.SegmentCode;
            customer.BiwExisting = hanaXjsPayload.BiwExisting;

            await _customerEventPublisher.Publish(c => new EventGridEvent(
                subject: customer.BenjaminMooreCustomerId,
                eventType: "customer-loyalty-created",
                dataVersion: "1.5",
                convertToEventGridCustomer(c))
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now
            }, customer);

            return new CustomerLoyaltyIndicator
            {
                BmcId = hanaXjsPayload.BmcId,
                LoyaltyIndicator = hanaXjsPayload.LoyaltyIndicator
            };
        }

        private EventGridCustomer convertToEventGridCustomer(Customer customer)
        {
            return new EventGridCustomer
            {
                bmc_id = customer.BenjaminMooreCustomerId,
                retailer_id = customer.RetailerId,
                loyalty_email_id = customer.LoyaltyEmailAddress,
                business_name = customer.BusinessName,
                business_email_id = customer.BusinessEmailAddress,
                business_phone_number = customer.BusinessPhoneNumber,
                business_type = customer.BusinessType,
                address_line1 = customer.Address1,
                address_line2 = customer.Address2,
                city = customer.City,
                state_code = customer.State,
                zipcode = customer.PostalCode,
                contact_first_name = customer.FirstName,
                contact_last_name = customer.LastName,
                contact_email_id = customer.ContactEmailAddress,
                contact_phone_number = customer.ContactPhoneNumber,
                segment_code = customer.SegmentCode,
                language_code = customer.LanguageCode,
                biw_existing = customer.BiwExisting
            };
        }
    }
}