using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing
{
    /// <summary>
    /// class for publishing a batch of documents
    /// </summary>
    public class EventGridClient : EventGridPublisherClient, IEventGridClient
    {
        public EventGridClient(Uri endpoint, AzureKeyCredential credential) : base(endpoint, credential)
        {

        }
        /// <summary>
        /// Publishes a batch of documents to a downstream store
        /// </summary>
        /// <param name="events">The batch of documents to publish.</param>
        /// <returns>Awaitable Task<Respone></returns>
        public async Task<Response> PublishEventsAsync(IList<EventGridEvent> events)
        {
             return (await SendEventsAsync(events));
        }
    }
}