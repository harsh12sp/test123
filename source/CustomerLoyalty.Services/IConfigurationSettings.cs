namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services
{
    public interface IConfigurationSettings
    {
        string KeyVaultCertificateName { get; }
        string KeyVaultBaseUri { get; }
        string CreateCustomerLoyaltyEndpoint { get; }
        string EventGridTopicUri { get; }
        string EventGridTopicKey { get; }
    }
}