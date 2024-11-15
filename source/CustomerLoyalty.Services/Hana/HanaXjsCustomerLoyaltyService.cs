using Azure.Messaging.EventGrid;
using BenjaminMoore.Api.Retail.Pos.Common.Dto;
using BenjaminMoore.Api.Retail.Pos.Common.Extensions;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaXjsCustomerLoyaltyService : ICustomerLoyaltyService
    {

        public async Task<CustomerLoyaltyIndicator> CreateCustomerLoyalty(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            var dummyResponse = new CustomerLoyaltyIndicator
            {
                Id = customer.CustomerId,
                LoyaltyIndicator = "Basic"
            };

            return await Task.FromResult(dummyResponse);
        }
    }
}