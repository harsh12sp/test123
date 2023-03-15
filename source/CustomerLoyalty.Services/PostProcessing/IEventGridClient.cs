using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing
{
    /// <summary>
    /// Interface for publishing a batch of documents
    /// </summary>
    public interface IEventGridClient
    {
        /// <summary>
        /// Publishes a batch of documents to a downstream store
        /// </summary>
        /// <param name="events">The batch of documents to publish.</param>
        /// <returns>Awaitable Task<Response></returns>
        public Task<Response> PublishEventsAsync(IList<EventGridEvent> events);
    }
}