using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using Moq;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Unit.PostProcessing
{
    public class AzureEventGridTopicPublisherTests
    {
        private readonly Mock<IEventGridClient> _eventGridClientMock;
        private readonly IEventPublisher<Customer> _customerPublisher;

        private static readonly Func<Customer, EventGridEvent> Converter = customer =>
            new EventGridEvent(
                subject: "test-subject",
                eventType: "customer-loyalty-created",
                dataVersion: "1.5",
                data: customer)
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow
            };

        public AzureEventGridTopicPublisherTests()
        {
            Mock<IConfigurationSettings> configurationSettingsMock = new Mock<IConfigurationSettings>();
            configurationSettingsMock.SetupGet(setup => setup.EventGridTopicUri).Returns("https://tempuri.local");
            _eventGridClientMock = new Mock<IEventGridClient>();
            _customerPublisher = new AzureEventGridTopicPublisher<Customer>(configurationSettingsMock.Object, _eventGridClientMock.Object);
        }

        [Theory]
        [ClassData(typeof(ConstructorData))]
        public void Ctor_WhenMissingConfigurationSettings_ShouldThrowAnArgumentNullException(IConfigurationSettings configurationSettings, IEventGridClient eventGridClient)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new AzureEventGridTopicPublisher<Customer>(configurationSettings, eventGridClient));
        }

        [Fact]
        public async Task Publish_WhenCustomerIsNull_ShouldThrowANullArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _customerPublisher.Publish(Converter, default(Customer)));
        }

        [Fact]
        public async Task Publish_WhenCustomerDocumentsArrayIsEmpty_ShouldThrowANullArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _customerPublisher.Publish(Converter));
        }

        [Fact]
        public async Task Publish_WhenCustomerDocumentsProvided_ShouldBatchCustomersToEventGrid()
        {
   
            var mockResponse = new Mock<Response>();
            mockResponse.SetupGet(x => x.Status).Returns((int)HttpStatusCode.OK);
            
            _eventGridClientMock.Setup(setup => setup.PublishEventsAsync(It.IsAny<IList<EventGridEvent>>()))
                .ReturnsAsync(mockResponse.Object);

            await _customerPublisher.Publish(Converter, new Customer());

            _eventGridClientMock.Verify(verify => verify.PublishEventsAsync(
                It.IsAny<IList<EventGridEvent>>()));
        }

        internal class ConstructorData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {new Mock<IConfigurationSettings>().Object, default(IEventGridClient)};
                yield return new object[] {default(IConfigurationSettings), new Mock<IEventGridClient>().Object};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}