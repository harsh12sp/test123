using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class CustomerLoyaltyIndicator
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("loyalty_indicator")]
        public string LoyaltyIndicator { get; set; }
    }
}