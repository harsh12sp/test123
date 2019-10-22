using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    /// <summary>
    /// Represents the data sent from a bad hana request.
    /// </summary>
    public class ErrorInfo
    {
        [JsonProperty("errors")]
        public JObject Errors { get; set; }

        [JsonProperty("info")]
        public ResponseMetadata ResponseInfo { get; set; }
    }
}