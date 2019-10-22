using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    /// <summary>
    /// Represents a set of flags that benjamin uses internally
    /// </summary>
    public class ResponseMetadata
    {
        /// <summary>
        /// Gets or sets the last updated date/time, when in use the expected format is ISO8601
        /// <remarks>
        /// Currently this will always be null and is merely fulfilling a contract.
        /// </remarks>
        /// </summary>
        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the response time in milliseconds
        /// </summary>
        [JsonProperty("response_time")]
        public string ResponseTime { get; set; }
    }
}