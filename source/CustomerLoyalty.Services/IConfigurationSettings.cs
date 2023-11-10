namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services
{
    public interface IConfigurationSettings
    {
        string KeyVaultCertificateName { get; }
        string KeyVaultBaseUri { get; }
        string CreateCustomerLoyaltyHanaBaseUrl { get; }
        string CreateCustomerLoyaltyHanaEndPoint { get; }
        string EventGridTopicUri { get; }
        string EventGridTopicKey { get; }
        string DefaultSegmentCode { get; }
        string DefaultLanguageCode { get; }
        string ErrorLogApiUrl { get; }
        string ErrorLogApiKey { get; }
        string LogBlobContainerName { get; }
        string LogDestination { get; }
        string LogDirectoryError { get; }
        string LogDirectoryDebug { get; }
        string LogDirectoryInfo { get; }
        string LogDirectoryWarning { get; }
    }
}