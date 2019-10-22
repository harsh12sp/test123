using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class AzureConfigurationSettingsTests
    {
        [Theory]
        [ClassData(typeof(ClassData))]
        public void Ctor_WhenAnEnvionrmentVariableIsEmpty_ShouldThrowAnArgumentNullException(Action setVariables)
        {
            // Arrange
            setVariables();

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => new AzureConfigurationSettings());
        }

        [Fact]
        public void AllGetters_WhenGotten_ShouldBeExpectedValues()
        {

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, AzureConfigurationSettings.KeyVaultBaseUriSettingName.Substring(1));

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyEndpointSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyEndpointSettingName.Substring(1));

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, AzureConfigurationSettings.KeyValueCertificateNameSettingName.Substring(1));

            AzureConfigurationSettings settings = new AzureConfigurationSettings();

            Assert.Equal(settings.KeyVaultBaseUri, AzureConfigurationSettings.KeyVaultBaseUriSettingName.Substring(1));
            Assert.Equal(settings.CreateCustomerLoyaltyEndpoint, AzureConfigurationSettings.CreateCustomerLoyaltyEndpointSettingName.Substring(1));
            Assert.Equal(settings.KeyVaultCertificateName, AzureConfigurationSettings.KeyValueCertificateNameSettingName.Substring(1));
        }


        internal class ClassData :IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Action(() => KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() => KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() => KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, null))
                };


                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyEndpointSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyEndpointSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyEndpointSettingName, null))
                };


                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, null))
                };
            }

            private void KnockoutEnvironmentVariable(string knockout, string value)
            {
                if (!knockout.Equals(AzureConfigurationSettings.KeyVaultBaseUriSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.CreateCustomerLoyaltyEndpointSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyEndpointSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.KeyValueCertificateNameSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, value);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
