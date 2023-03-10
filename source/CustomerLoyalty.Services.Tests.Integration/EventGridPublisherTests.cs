﻿using System;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.PostProcessing;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Integration
{
    public class EventGridPublisherTests
    {
        [Fact(Skip = "Manual Test for Ensuring Queue Messages")]
        public async Task Publish_SendEventsTest()
        {
            IConfigurationSettings settings = Settings.GetFromAppSettings<EventGridSettings>();
            IEventGridClient client = new EventGridClient(new Uri(settings.EventGridTopicUri),new AzureKeyCredential(settings.EventGridTopicKey));

            AzureEventGridTopicPublisher<Customer> customerPublisher =
                new AzureEventGridTopicPublisher<Customer>(settings, client);

            await customerPublisher.Publish(c => new EventGridEvent(
                subject: c.BenjaminMooreCustomerId,
                eventType: "customer-loyalty-created",
                dataVersion: "1.5",
                data: c)
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now
            }, new TestCustomer());
        }

        internal class TestCustomer : Customer
        {
            public TestCustomer()
            {
                Address1 = "123 Test Run";
                Address2 = "Apartment T";
                BenjaminMooreCustomerId = "ag1";
                BusinessEmailAddress = "test@test.test";
                BusinessName = "Test";
                BusinessPhoneNumber = "5555551212";
                BusinessType = "Test";
                City = "Test";
                ContactEmailAddress = "test@test.test";
                ContactPhoneNumber = "5555551212";
                FirstName = "Test";
                LastName = "Test";
                RetailerId = "Test";
                State = "NJ";
                LoyaltyEmailAddress = "test@test.test";
                PostalCode = "12345";
                SegmentCode = "CODE";
                BiwExisting = "false";         
            }
        }
    }
}