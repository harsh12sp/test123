using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class Customer
    {
        [JsonProperty("id")]
        public string CustomerId { get; set; }

        [JsonProperty("contact_first_name")]
        public string FirstName { get; set; }

        [JsonProperty("contact_last_name")]
        public string LastName { get; set; }

        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }
    }
}