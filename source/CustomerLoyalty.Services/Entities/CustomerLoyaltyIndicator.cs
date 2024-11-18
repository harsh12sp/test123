using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class CustomerLoyaltyIndicator
    {
        [JsonProperty("response")]
        public Todo Reponse { get; set; }
    }
}