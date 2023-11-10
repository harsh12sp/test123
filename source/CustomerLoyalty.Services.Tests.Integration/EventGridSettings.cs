namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Integration
{
    internal class EventGridSettings : IConfigurationSettings
    {
        public string Token { get; set; }
        public string Uri { get; set; }
        public string KeyVaultCertificateName => "Missing";
        public string KeyVaultBaseUri => "Missing";
        public string CreateCustomerLoyaltyHanaBaseUrl => "Missing";
        public string CreateCustomerLoyaltyHanaEndPoint => "Missing";
        public string EventGridTopicUri => Uri;
        public string EventGridTopicKey => Token;
        public string DefaultSegmentCode => "Missing";
        public string DefaultLanguageCode => "Missing";

        public string ErrorLogApiUrl => "Missing";

        public string ErrorLogApiKey => "Missing";

        public string LogBlobContainerName => "Missing";

        public string LogDestination => "Missing";

        public string LogDirectoryError => "Missing";

        public string LogDirectoryDebug => "Missing";

        public string LogDirectoryInfo => "Missing";

        public string LogDirectoryWarning => "Missing";
    }
}