using System;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services
{
    public class AzureConfigurationSettings : IConfigurationSettings
    {
        public const string KeyValueCertificateNameSettingName = "KeyVault.CertificateName";
        public const string KeyVaultBaseUriSettingName = "KeyVault.BaseUri";
        public const string CreateCustomerLoyaltyHanaBaseUrlSettingName = "Hana.CreateCustomerLoyaltyBaseUrl";
        public const string CreateCustomerLoyaltyHanaEndPointSettingName = "Hana.CreateCustomerLoyaltyEndPoint";
        public const string EventGridTopicUriSettingName = "EventGrid.TopicUri";
        public const string EventGridTopicKeySettingName = "EventGrid.Key";
        public const string DefaultSegmentCodeSettingName = "Defaults.SegmentCode";
        public const string DefaultLanguageCodeSettingName = "Defaults.LanguageCode";
        private const string UnconfiguredDefaultSegmentCode = "TRDCPAINT";
        private const string UnconfiguredDefaultLanguageCode = "EN";

        public AzureConfigurationSettings()
        {
            KeyVaultCertificateName = Environment.GetEnvironmentVariable(KeyValueCertificateNameSettingName);
            KeyVaultBaseUri = Environment.GetEnvironmentVariable(KeyVaultBaseUriSettingName);
            CreateCustomerLoyaltyHanaBaseUrl =
                Environment.GetEnvironmentVariable(CreateCustomerLoyaltyHanaBaseUrlSettingName);
            CreateCustomerLoyaltyHanaEndPoint =
                Environment.GetEnvironmentVariable(CreateCustomerLoyaltyHanaEndPointSettingName);
            EventGridTopicUri = Environment.GetEnvironmentVariable(EventGridTopicUriSettingName);
            EventGridTopicKey = Environment.GetEnvironmentVariable(EventGridTopicKeySettingName);

            DefaultSegmentCode =
                Environment.GetEnvironmentVariable(DefaultSegmentCodeSettingName) ?? UnconfiguredDefaultSegmentCode;

            DefaultLanguageCode = Environment.GetEnvironmentVariable(DefaultLanguageCodeSettingName) ??
                                  UnconfiguredDefaultLanguageCode;

            if (string.IsNullOrWhiteSpace(KeyVaultCertificateName))
            {
                throw new ArgumentNullException(KeyValueCertificateNameSettingName);
            }

            if (string.IsNullOrWhiteSpace(KeyVaultBaseUri))
            {
                throw new ArgumentNullException(KeyVaultBaseUriSettingName);
            }

            if (string.IsNullOrWhiteSpace(CreateCustomerLoyaltyHanaBaseUrl))
            {
                throw new ArgumentNullException(CreateCustomerLoyaltyHanaBaseUrlSettingName);
            }

            if (string.IsNullOrWhiteSpace(CreateCustomerLoyaltyHanaEndPoint))
            {
                throw new ArgumentNullException(CreateCustomerLoyaltyHanaEndPointSettingName);
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
        public string CreateCustomerLoyaltyHanaBaseUrl { get; }
        public string EventGridTopicUri { get; }
        public string EventGridTopicKey { get; }
        public string DefaultSegmentCode { get; }
        public string DefaultLanguageCode { get; }
        public string CreateCustomerLoyaltyHanaEndPoint { get; }
    }
}