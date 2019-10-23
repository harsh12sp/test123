using System;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing
{
    /// <summary>
    /// Interface for publishing a batch of documents
    /// </summary>
    /// <typeparam name="TEventPayload">The type of documents to publish</typeparam>
    public interface IEventPublisher<TEventPayload>
    {
        /// <summary>
        /// Publishes a batch of documents to a downstream store
        /// </summary>
        /// <param name="payloadToEventConverter"></param>
        /// <param name="events">The batch of documents to publish.</param>
        /// <returns>Awaitable Task</returns>
        Task Publish(Func<TEventPayload, EventGridEvent> payloadToEventConverter, params TEventPayload[] events);
    }
}
