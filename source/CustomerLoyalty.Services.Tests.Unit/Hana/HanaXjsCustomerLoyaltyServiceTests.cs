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
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using Microsoft.Azure.EventGrid.Models;
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
        private readonly Mock<IEventPublisher<Customer>> _customerPublisherMock;

        public HanaXjsCustomerLoyaltyServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationSettingsMock = new Mock<IConfigurationSettings>();
            _customerPublisherMock = new Mock<IEventPublisher<Customer>>();

            _configurationSettingsMock.SetupGet(setup => setup.CreateCustomerLoyaltyEndpoint)
                .Returns("https://mock.local");

            _configurationSettingsMock.SetupGet(setup => setup.DefaultLanguageCode)
                .Returns("DLG");

            _customerLoyaltyService =
                new HanaXjsCustomerLoyaltyService(_httpClientFactoryMock.Object, _configurationSettingsMock.Object, _customerPublisherMock.Object);
        }

        [Theory]
        [ClassData(typeof(ConstructorClassData))]
        public void Ctor_WhenCalledWithInvalidConstructorData_ShouldThrowArgumentNullException(
            IHttpClientFactory clientFactory, IConfigurationSettings configurationSettings, IEventPublisher<Customer> customerPublisher)
        {
            // AAA
            Assert.Throws<ArgumentNullException>(() =>
                new HanaXjsCustomerLoyaltyService(clientFactory, configurationSettings, customerPublisher));
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
            // Arrange
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient()).ReturnsAsync(new HttpClient());

            try
            {
                // Act
                await _customerLoyaltyService.CreateCustomerLoyalty(default(Customer));
            }
            catch (ArgumentNullException)
            {
                // expected exception
            }

            // Assert
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

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerIsPosted_ShouldCallThePublisher()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                        new HanaXjsCreateLoyaltyResponse())));

            Customer customer = new Customer();

            _customerPublisherMock.Setup(setup => setup.Publish(It.IsAny<Func<Customer, EventGridEvent>>(), It.IsAny<Customer>())).Returns(Task.CompletedTask)
                .Callback<Func<Customer, EventGridEvent>, Customer[]>((converter, customerArg) =>
                {
                    // Assert -- Ensure the customer we passed in is the customer returned?
                    Assert.Contains(customer, customerArg);
                });

            await _customerLoyaltyService.CreateCustomerLoyalty(customer);

            // Assert
            _customerPublisherMock.Verify(verify => verify.Publish(It.IsAny<Func<Customer, EventGridEvent>>(), It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenHanaSays400_ShouldThrowAHanaRequestException()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<string>(HttpStatusCode.BadRequest,
                        "\"reason\":\"some hana error\"")));

            await Assert.ThrowsAsync<HanaRequestException>(async () =>
                await _customerLoyaltyService.CreateCustomerLoyalty(new Customer()));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CreateCustomerLoyalty_WhenCustomerIsMissingLanguageCode_ShouldUseDefaultCode(string code)
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                        new HanaXjsCreateLoyaltyResponse())));

            CustomerLoyaltyIndicator customerLoyaltyIndicator =
                await _customerLoyaltyService.CreateCustomerLoyalty(new Customer{LanguageCode = code});

            _configurationSettingsMock.VerifyGet(verify => verify.DefaultLanguageCode, Times.Once);

            Assert.NotNull(customerLoyaltyIndicator);
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenCustomerLanguageCodeIsProvided_ShouldNotUseDefaultCode()
        {
            _httpClientFactoryMock.Setup(setup => setup.CreateHttpClient())
                .ReturnsAsync(new TestHttpClient(
                    new StatusCodeMessageHandler<HanaXjsCreateLoyaltyResponse>(HttpStatusCode.OK,
                        new HanaXjsCreateLoyaltyResponse())));

            CustomerLoyaltyIndicator customerLoyaltyIndicator =
                await _customerLoyaltyService.CreateCustomerLoyalty(new Customer {LanguageCode = "EN"});

            _configurationSettingsMock.VerifyGet(verify => verify.DefaultLanguageCode, Times.Never);

            Assert.NotNull(customerLoyaltyIndicator);
        }

        internal class ConstructorClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    default(IHttpClientFactory),
                    new Mock<IConfigurationSettings>().Object,
                    new Mock<IEventPublisher<Customer>>().Object
                };

                yield return new object[]
                {
                    new Mock<IHttpClientFactory>().Object,
                    default(IConfigurationSettings),
                    new Mock<IEventPublisher<Customer>>().Object
                };
                
                yield return new object[]
                {
                    new Mock<IHttpClientFactory>().Object,
                    new Mock<IConfigurationSettings>().Object,
                    default(IEventPublisher<Customer>)
                };
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
