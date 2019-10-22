using System;
using BenjaminMoore.Api.Retail.Pos.Common.Authentication;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly:FunctionsStartup(typeof(Startup))]
namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // This should be the only location outside of AzureConfigurationSettings class where we directly read settings.
            string env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(env) || env != "Development")
            {
                ConfigureProduction(builder);
            }
            else
            {
                ConfigureLocalDevelopment(builder);
            }
        }

        private void ConfigureProduction(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IConfigurationSettings, AzureConfigurationSettings>();
            builder.Services.AddSingleton<ICertificateLocation>(provider =>
            {
                IConfigurationSettings settings = provider.GetRequiredService<IConfigurationSettings>();
                return new AzureKeyVaultCertificateLocation(settings.KeyVaultCertificateName, settings.KeyVaultBaseUri);
            });

            builder.Services.AddSingleton<ICertificateValidator, MutualTlsCertificateValidator>();
            builder.Services.AddSingleton<ICertificateRetriever, AzureKeyVaultCertificateRetriever>();
            builder.Services.AddSingleton<IHttpClientFactory, MutualTlsHttpClientFactory>();

            // NB: This will only run on a vnet peered with on-premises access to ERP-SAP system.
            builder.Services.AddTransient<ICustomerLoyaltyService, HanaXjsCustomerLoyaltyService>();
        }

        private void ConfigureLocalDevelopment(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICustomerLoyaltyService, FakeCustomerLoyaltyService>();
        }
    }
}
