using Newtonsoft.Json;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class Customer
    {
        [JsonProperty("bmc_id")]
        public string BenjaminMooreCustomerId { get; set; }

        [JsonProperty("retailer_id")]
        public string RetailerId { get; set; }

        [JsonProperty("loyalty_email_id")]
        public string LoyaltyEmailAddress { get; set; }

        [JsonProperty("business_name")]
        public string BusinessName { get; set; }

        [JsonProperty("business_email_id")]
        public string BusinessEmailAddress { get; set; }

        [JsonProperty("business_phone_number")]
        public string BusinessPhoneNumber { get; set; }

        [JsonProperty("business_type")]
        public string BusinessType { get; set; }

        [JsonProperty("address_line1")]
        public string Address1 { get; set; }

        [JsonProperty("address_line2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state_code")]
        public string State { get; set; }

        [JsonProperty("zipcode")]
        public string PostalCode { get; set; }

        [JsonProperty("contact_first_name")]
        public string FirstName { get; set; }

        [JsonProperty("contact_last_name")]
        public string LastName { get; set; }

        [JsonProperty("contact_email_id")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("contact_phone_number")]
        public string ContactPhoneNumber { get; set; }

        [JsonProperty("segment_code")]
        public string SegmentCode { get; set; }

        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }

        [JsonProperty("biw_existing")]
        public string BiwExisting {get; set;}
    }
}