using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class Customer
    {
        [JsonProperty("bmc_id")]
        public string BenjaminMooreCustomerId { get; set; }

        [JsonProperty("retailer_id")]
        public string RetailerId { get; set; }

        [JsonProperty("business_name")]
        public string BusinessName { get; set; }

        [JsonProperty("business_email_id")]
        public string BusinessEmailAddress { get; set; }

        [JsonProperty("loyalty_email_id")]
        public string LoyaltyEmailAddress { get; set; }

        [JsonProperty("business_phone_number")]
        public string BusinessPhoneNumber { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("address_line_1")]
        public string Address1 { get; set; }

        [JsonProperty("address_line_2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string PostalCode { get; set; }

        [JsonProperty("business_type")]
        public string BusinessType { get; set; }

        [JsonProperty("outlet")]
        public string Outlet { get; set; }

        [JsonProperty("contact_email_id")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("contact_phone_number")]
        public string ContactPhoneNumber { get; set; }
    }
}