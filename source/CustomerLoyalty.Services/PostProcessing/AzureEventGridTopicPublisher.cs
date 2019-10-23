using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing
{
    public class AzureEventGridTopicPublisher<TEventPayload> : IEventPublisher<TEventPayload>
    {
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IEventGridClient _eventGridClient;

        public AzureEventGridTopicPublisher(IConfigurationSettings configurationSettings, IEventGridClient eventGridClient)
        {
            _configurationSettings = configurationSettings ?? throw new ArgumentNullException(nameof(configurationSettings));
            _eventGridClient = eventGridClient ?? throw new ArgumentNullException(nameof(eventGridClient));
        }

        public Task Publish(Func<TEventPayload, EventGridEvent> payloadConverter, params TEventPayload[] events)
        {
            if (events == null || events.All(c => c == null)) throw new ArgumentNullException(nameof(events));

            string eventGridHost = new Uri(_configurationSettings.EventGridTopicUri).Host;

            return _eventGridClient.PublishEventsAsync(eventGridHost, events.Select(payloadConverter).ToArray());
        }
    }
}