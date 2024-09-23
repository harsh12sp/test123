using System;
using Azure;
using BenjaminMoore.Api.Retail.Pos.Common.Authentication;
using BenjaminMoore.Api.Retail.Pos.Common.Configuration;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.Common.Logger;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Utils;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using BenjaminMoore.Api.Retail.Pos.Customers.FunctionApp.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((services) =>
    {
        services.AddMvc().AddNewtonsoftJson();

        //This should be the only location outside of AzureConfigurationSettings class where we directly read settings.
        string env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

        if (string.IsNullOrWhiteSpace(env) || env != "Development")
        {
            ConfigureProduction(services);
        }
        else
        {
            ConfigureLocalDevelopment(services);
        }
    })
    .Build();

host.Run();


void ConfigureProduction(IServiceCollection services)
{
    services.AddSingleton<IConfigurationSettings, AzureConfigurationSettings>();
    services.AddSingleton<ICertificateLocation>(provider =>
    {
        IConfigurationSettings settings = provider.GetRequiredService<IConfigurationSettings>();
        return new AzureKeyVaultCertificateLocation(settings.KeyVaultCertificateName, settings.KeyVaultBaseUri);
    });

    services.AddSingleton<IEventGridClient>(provider =>
    {
        IConfigurationSettings settings = provider.GetRequiredService<IConfigurationSettings>();
        AzureKeyCredential credentials = new AzureKeyCredential(settings.EventGridTopicKey);
        EventGridClient client = new EventGridClient(new Uri(settings.EventGridTopicUri), credentials);

        return client;
    });

    services.AddSingleton<ILoggerConfigurationSettings>(provider =>
    {
        IConfigurationSettings settings = provider.GetRequiredService<IConfigurationSettings>();
        return new LoggerConfigurationSettings(settings.ErrorLogApiKey, settings.ErrorLogApiUrl, settings.LogDestination, settings.LogBlobContainerName, settings.LogDirectoryError, settings.LogDirectoryDebug, settings.LogDirectoryInfo, settings.LogDirectoryWarning);
    });

    services.AddSingleton<IEventPublisher<Customer>, AzureEventGridTopicPublisher<Customer>>();
    services.AddSingleton<ICertificateValidator, MutualTlsCertificateValidator>();
    services.AddSingleton<ICertificateRetriever, AzureKeyVaultCertificateRetriever>();
    services.AddSingleton<ILoggerHttpClientFactory, LoggerHttpClientFactory>();
    services.AddSingleton<IHttpClientFactory, MutualTlsHttpClientFactory>();

    services.AddSingleton<ILoggerService, LoggerService>();
    services.AddSingleton<IErrorHandler, ErrorHandler>();

    // NB: This will only run on a vnet peered with on-premises access to ERP-SAP system.
    services.AddTransient<ICustomerLoyaltyService, HanaXjsCustomerLoyaltyService>();
}

void ConfigureLocalDevelopment(IServiceCollection services)
{
    services.AddSingleton<IErrorHandler, TestFakeErrorHandler>();
    services.AddSingleton<ICustomerLoyaltyService, FakeCustomerLoyaltyService>();
}