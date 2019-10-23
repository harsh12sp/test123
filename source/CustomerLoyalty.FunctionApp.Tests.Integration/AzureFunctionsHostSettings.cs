using System.Diagnostics.CodeAnalysis;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class AzureFunctionsHostSettings
    {
        public string FunctionHostPath { get; set; }
        public string FunctionAppPath { get; set; }
    }
}