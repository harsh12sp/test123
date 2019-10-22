using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    /// <summary>
    /// Represents a the customer loyalty info with response metadata.
    /// </summary>
    public class CustomerInfo
    {
        [JsonProperty("data")]
        public CustomerLoyaltyIndicator CustomerLoyaltyIndicator { get; set; }
        [JsonProperty("info")]
        public ResponseMetadata ResponseInfo { get; set; }
    }
}