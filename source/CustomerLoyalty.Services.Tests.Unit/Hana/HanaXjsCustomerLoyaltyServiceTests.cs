using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.Common.Http;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana.Entities;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Unit.Hana
{
    [ExcludeFromCodeCoverage]
    public class HanaXjsCustomerLoyaltyServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfigurationSettings> _configurationSettingsMock;
        private readonly ICustomerLoyaltyService _customerLoyaltyService;

        public HanaXjsCustomerLoyaltyServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationSettingsMock = new Mock<IConfigurationSettings>();

            _configurationSettingsMock.SetupGet(setup => setup.CreateCustomerLoyaltyEndpoint)
                .Returns("https://mock.local");

            _customerLoyaltyService =
                new HanaXjsCustomerLoyaltyService(_httpClientFactoryMock.Object, _configurationSettingsMock.Object);
        }

        [Theory]
        [ClassData(typeof(ConstructorClassData))]
        public void Ctor_WhenCalledWithInvalidConstructorData_ShouldThrowArgumentNullException(
            IHttpClientFactory clientFactory, IConfigurationSettings configurationSettings)
        {
            // AAA
            Assert.Throws<ArgumentNullException>(() =>
                new HanaXjsCustomerLoyaltyService(clientFactory, configurationSettings));
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsNull_ShouldThrowANullArgumentException()
        {
            // AAA
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _customerLoyaltyService.CreateCustomerLoyalty(default(Customer)));
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsNull_ShouldNotCreateHttpClient()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient()).ReturnsAsync(new HttpClient());

            try
            {
                await _customerLoyaltyService.CreateCustomerLoyalty(default(Customer));
            }
            catch (ArgumentNullException)
            {
                // expected exception
            }

            _httpClientFactoryMock.Verify(verify => verify.CreateHttpClient(), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsNotNull_ShouldCallFactoryCreateMethod()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient()).ReturnsAsync(
                new TestHttpClient(new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                    new HanaXjsCreateLoyaltyResponse())));


            await _customerLoyaltyService.CreateCustomerLoyalty(new Customer());


            _httpClientFactoryMock.Verify(verify => verify.CreateHttpClient(), Times.Once);
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsValid_ShouldGetUriFromConfiguratioObject()
        {
            // Arrange
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                        new HanaXjsCreateLoyaltyResponse())));

            // Act
            await _customerLoyaltyService.CreateCustomerLoyalty(new Customer());

            // Assert
            _configurationSettingsMock.VerifyGet(verify => verify.CreateCustomerLoyaltyEndpoint, Times.Once);
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsValid_ShouldReturnACustomerLoyalty()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                        new HanaXjsCreateLoyaltyResponse())));

            CustomerLoyaltyIndicator customerLoyaltyIndicator =
                await _customerLoyaltyService.CreateCustomerLoyalty(new Customer());

            Assert.NotNull(customerLoyaltyIndicator);
        }


        internal class ConstructorClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {new Mock<IHttpClientFactory>().Object, default(IConfigurationSettings)};
                yield return new object[] {default(IHttpClientFactory), new Mock<IConfigurationSettings>().Object};
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        internal class TestHttpClient : HttpClient
        {
            public TestHttpClient(HttpMessageHandler messageHandler) : base(messageHandler)
            {

            }
        }

        internal class StatusCodeMessageHandler<T> : HttpMessageHandler where T : class
        {
            private readonly HttpStatusCode _statusCode;
            private readonly T _payload;

            public StatusCodeMessageHandler(HttpStatusCode statusCode, T payload)
            {
                _statusCode = statusCode;
                _payload = payload;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                if (_payload != null)
                {
                    return Task.FromResult(new HttpResponseMessage(_statusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(_payload), Encoding.UTF8,
                            "application/json")
                    });
                }

                return Task.FromResult(new HttpResponseMessage(_statusCode));
            }
        }
    }
}
