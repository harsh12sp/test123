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

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, AzureConfigurationSettings.KeyVaultBaseUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, AzureConfigurationSettings.KeyValueCertificateNameSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, AzureConfigurationSettings.EventGridTopicUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, AzureConfigurationSettings.EventGridTopicKeySettingName);

            AzureConfigurationSettings settings = new AzureConfigurationSettings();

            Assert.Equal(settings.KeyVaultBaseUri, AzureConfigurationSettings.KeyVaultBaseUriSettingName);
            Assert.Equal(settings.CreateCustomerLoyaltyHanaBaseUrl, AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName);
            Assert.Equal(settings.CreateCustomerLoyaltyHanaEndPoint, AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName);
            Assert.Equal(settings.KeyVaultCertificateName, AzureConfigurationSettings.KeyValueCertificateNameSettingName);
            Assert.Equal(settings.EventGridTopicKey, AzureConfigurationSettings.EventGridTopicKeySettingName);
            Assert.Equal(settings.EventGridTopicUri, AzureConfigurationSettings.EventGridTopicUriSettingName);
        }

        [Fact]
        public void DefaultSegmentCode_WhenGotten_ShouldBeExpectedValues()
        {

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, AzureConfigurationSettings.KeyVaultBaseUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, AzureConfigurationSettings.KeyValueCertificateNameSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, AzureConfigurationSettings.EventGridTopicUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, AzureConfigurationSettings.EventGridTopicKeySettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.DefaultSegmentCodeSettingName, AzureConfigurationSettings.DefaultSegmentCodeSettingName);


            AzureConfigurationSettings settings = new AzureConfigurationSettings();

            Assert.Equal(settings.DefaultSegmentCode, AzureConfigurationSettings.DefaultSegmentCodeSettingName);
        }

        [Fact]
        public void DefaultLanguageCode_WhenGotten_ShouldBeExpectedValues()
        {

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, AzureConfigurationSettings.KeyVaultBaseUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName,
                AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, AzureConfigurationSettings.KeyValueCertificateNameSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, AzureConfigurationSettings.EventGridTopicUriSettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, AzureConfigurationSettings.EventGridTopicKeySettingName);

            Environment.SetEnvironmentVariable(AzureConfigurationSettings.DefaultLanguageCodeSettingName, AzureConfigurationSettings.DefaultLanguageCodeSettingName);


            AzureConfigurationSettings settings = new AzureConfigurationSettings();

            Assert.Equal(settings.DefaultLanguageCode, AzureConfigurationSettings.DefaultLanguageCodeSettingName);
        }

        internal class ClassData :IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // knockout config for key vault baseuri
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


                // knockout config for baseUrl
                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaBaseUrlSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaBaseUrlSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaBaseUrlSettingName, null))
                };

                // knockout config for endpoint
                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaEndPointSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaEndPointSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings
                            .CreateCustomerLoyaltyHanaEndPointSettingName, null))
                };

                // knockout config for key vault certificate
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

                // knockout config for event grid topic key
                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, null))
                };

                //knockout config for event grid host
                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, ""))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, " "))
                };

                yield return new object[]
                {
                    new Action(() =>
                        KnockoutEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, null))
                };
            }

            private void KnockoutEnvironmentVariable(string knockout, string value)
            {
                if (!knockout.Equals(AzureConfigurationSettings.KeyVaultBaseUriSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyVaultBaseUriSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaBaseUrlSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.CreateCustomerLoyaltyHanaEndPointSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.KeyValueCertificateNameSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.KeyValueCertificateNameSettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.EventGridTopicKeySettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicKeySettingName, value);
                }

                if (!knockout.Equals(AzureConfigurationSettings.EventGridTopicUriSettingName))
                {
                    Environment.SetEnvironmentVariable(AzureConfigurationSettings.EventGridTopicUriSettingName, value);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
