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
        private const string ErrorLogAPIUrlSettingName = "ErrorLog.ApiUrl";
        private const string ErrorLogAPIKeySettingName = "ErrorLog.ApiKey";
        private const string LogBlobContainerNameSettingName = "ErrorLog.BlobContainerName";
        private const string LogDestinationSettingName = "ErrorLog.Destination";
        private const string LogDirectoryErrorSettingName = "ErrorLog.ErrorDirectoryName";
        private const string LogDirectoryDebugSettingName = "ErrorLog.DebugDirectoryName";
        private const string LogDirectoryInfoSettingName = "ErrorLog.InfoDirectoryName";
        private const string LogDirectoryWarningSettingName = "ErrorLog.WarningDirectoryName";

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

            ErrorLogApiUrl = Environment.GetEnvironmentVariable(ErrorLogAPIUrlSettingName);
            ErrorLogApiKey = Environment.GetEnvironmentVariable(ErrorLogAPIKeySettingName);
            LogBlobContainerName = Environment.GetEnvironmentVariable(LogBlobContainerNameSettingName);
            LogDestination = Environment.GetEnvironmentVariable(LogDestinationSettingName);
            LogDirectoryError = Environment.GetEnvironmentVariable(LogDirectoryErrorSettingName);
            LogDirectoryDebug = Environment.GetEnvironmentVariable(LogDirectoryDebugSettingName);
            LogDirectoryInfo = Environment.GetEnvironmentVariable(LogDirectoryInfoSettingName);
            LogDirectoryWarning = Environment.GetEnvironmentVariable(LogDirectoryWarningSettingName);

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
        public string ErrorLogApiUrl { get; }
        public string ErrorLogApiKey { get; }
        public string LogBlobContainerName { get; }
        public string LogDestination { get; }
        public string LogDirectoryError { get; }
        public string LogDirectoryDebug { get; }
        public string LogDirectoryInfo { get; }
        public string LogDirectoryWarning { get; }
    }
}