/*
    Create the project:
        dotnet new console --name EventHubClient

    Add references to the project:
        dotnet add EventHubClient package Azure.Messaging.EventHubs

    Run the project:
        dotnet run --project EventHubClient

    See: https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send
*/

using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace EventHubClient
{
    class Program
    {
        // connection string to the Event Hubs namespace
        private const string connectionString = "Endpoint=sb://ehns00001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iQiMcx/WKGzJlazWQXd3HzgGbEyUOHHiM8zp1YDr1Fw=";

        // name of the event hub
        private const string eventHubName = "myhub";

        // number of events to be sent to the event hub
        private const int numOfEvents = 5;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        static EventHubProducerClient producerClient;

        static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(connectionString, eventHubName);

            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            for (int i = 1; i <= numOfEvents; i++)
            {
                string eventData = $"Event {i}";
                await Console.Out.WriteLineAsync($"Add event to the batch: {eventData}");

                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventData))))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                await Console.Out.WriteLineAsync($"Sending batch of {numOfEvents} events...");

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                await Console.Out.WriteLineAsync($"A batch of {numOfEvents} events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
    }
}
