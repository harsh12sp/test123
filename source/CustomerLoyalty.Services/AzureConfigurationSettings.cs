using System;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services
{
    public class AzureConfigurationSettings : IConfigurationSettings
    {
        public const string KeyValueCertificateNameSettingName = "KeyVault.CertificateName";
        public const string KeyVaultBaseUriSettingName = "KeyVault.BaseUri";
        public const string CreateCustomerLoyaltyEndpointSettingName = "Hana.CreateCustomerLoyaltyEndpoint";
        public const string EventGridTopicUriSettingName = "EventGrid.TopicUri";
        public const string EventGridTopicKeySettingName = "EventGrid.Key";

        public AzureConfigurationSettings()
        {
            KeyVaultCertificateName = Environment.GetEnvironmentVariable(KeyValueCertificateNameSettingName);
            KeyVaultBaseUri = Environment.GetEnvironmentVariable(KeyVaultBaseUriSettingName);
            CreateCustomerLoyaltyEndpoint =
                Environment.GetEnvironmentVariable(CreateCustomerLoyaltyEndpointSettingName);
            EventGridTopicUri = Environment.GetEnvironmentVariable(EventGridTopicUriSettingName);
            EventGridTopicKey = Environment.GetEnvironmentVariable(EventGridTopicKeySettingName);

            if (string.IsNullOrWhiteSpace(KeyVaultCertificateName))
            {
                throw new ArgumentNullException(KeyValueCertificateNameSettingName);
            }

            if (string.IsNullOrWhiteSpace(KeyVaultBaseUri))
            {
                throw new ArgumentNullException(KeyVaultBaseUriSettingName);
            }

            if (string.IsNullOrWhiteSpace(CreateCustomerLoyaltyEndpoint))
            {
                throw new ArgumentNullException(CreateCustomerLoyaltyEndpointSettingName);
            }

            if (string.IsNullOrWhiteSpace(EventGridTopicUri))
            {
                throw new ArgumentNullException(EventGridTopicUriSettingName);
            }

            if (string.IsNullOrWhiteSpace(EventGridTopicKey))
            {
                throw new ArgumentNullException(EventGridTopicKeySettingName);
            }
        }

        public string KeyVaultCertificateName { get; }
        public string KeyVaultBaseUri { get; }
        public string CreateCustomerLoyaltyEndpoint { get; }
        public string EventGridTopicUri { get; }
        public string EventGridTopicKey { get; }
    }
}