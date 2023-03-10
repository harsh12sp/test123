namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities
{
    public class EventGridCustomer
    {
        public string bmc_id { get; set; }
        public string retailer_id { get; set; }
        public string loyalty_email_id { get; set; }
        public string business_name { get; set; }
        public string business_email_id { get; set; }
        public string business_phone_number { get; set; }
        public string business_type { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string state_code { get; set; }
        public string zipcode { get; set; }
        public string contact_first_name { get; set; }
        public string contact_last_name { get; set; }
        public string contact_email_id { get; set; }
        public string contact_phone_number { get; set; }
        public string segment_code { get; set; }
        public string language_code { get; set; }
        public string biw_existing {get; set;}
    }
}