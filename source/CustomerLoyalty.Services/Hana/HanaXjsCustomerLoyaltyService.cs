
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaXjsCustomerLoyaltyService : ICustomerLoyaltyService
    {

        public async Task<CustomerLoyaltyIndicator> CreateCustomerLoyalty(Customer customer)
        {

            using (var client = new HttpClient())
            {
                var url = "https://jsonplaceholder.typicode.com/todos/1";

                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<Todo>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var customerLoyaltyIndicator = new CustomerLoyaltyIndicator
                {
                    Reponse = apiResponse
                };

                return customerLoyaltyIndicator;
            }
        }
    }

    public class Todo
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}