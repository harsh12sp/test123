using System;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services
{
    public class AzureConfigurationSettings : IConfigurationSettings
    {
        public const string KeyValueCertificateNameSettingName = "KeyVault.CertificateName";
        public const string KeyVaultBaseUriSettingName = "KeyVault.BaseUri";
        public const string CreateCustomerLoyaltyEndpointSettingName = "Hana.CreateCustomerLoyaltyEndpoint";

        public AzureConfigurationSettings()
        {
            KeyVaultCertificateName = Environment.GetEnvironmentVariable(KeyValueCertificateNameSettingName);
            KeyVaultBaseUri = Environment.GetEnvironmentVariable(KeyVaultBaseUriSettingName);
            CreateCustomerLoyaltyEndpoint =
                Environment.GetEnvironmentVariable(CreateCustomerLoyaltyEndpointSettingName);

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
        }

        public string KeyVaultCertificateName { get; }
        public string KeyVaultBaseUri { get; }
        public string CreateCustomerLoyaltyEndpoint { get; }
    }
}