using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest.Azure;
using Moq;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Unit.PostProcessing
{
    public class AzureEventGridTopicPublisherTests
    {
        private readonly Mock<IEventGridClient> _eventGridClientMock;
        private readonly IEventPublisher<Customer> _customerPublisher;

        private static readonly Func<Customer, EventGridEvent> Converter = customer =>
            new EventGridEvent {Id = Guid.NewGuid().ToString(), Data = customer};

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
            _eventGridClientMock.Setup(setup => setup.PublishEventsWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<IList<EventGridEvent>>(), It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AzureOperationResponse());

            await _customerPublisher.Publish(Converter, new Customer());

            _eventGridClientMock.Verify(verify => verify.PublishEventsWithHttpMessagesAsync(It.IsAny<string>(),
                It.IsAny<IList<EventGridEvent>>(), It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<CancellationToken>()));
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