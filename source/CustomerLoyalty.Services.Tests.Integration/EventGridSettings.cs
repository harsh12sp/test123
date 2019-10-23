﻿namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Integration
{
    internal class EventGridSettings : IConfigurationSettings
    {
        public string Token { get; set; }
        public string Uri { get; set; }
        public string KeyVaultCertificateName => "Missing";
        public string KeyVaultBaseUri => "Missing";
        public string CreateCustomerLoyaltyEndpoint => "Missing";
        public string EventGridTopicUri => Uri;
        public string EventGridTopicKey => Token;
    }
}