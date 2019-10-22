using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class CustomerLoyaltyIndicator
    {
        [JsonProperty("bmc_id")]
        public string BmcId { get; set; }

        [JsonProperty("loyalty_indicator")]
        public string LoyaltyIndicator { get; set; }
    }
}