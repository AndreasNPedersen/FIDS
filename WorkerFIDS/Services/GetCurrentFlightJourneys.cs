using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;
using WorkerFIDS.Models;

namespace WorkerFIDS.Services
{
    //Sends rabbit messages to renew the cache
    public static class GetCurrentFlightJourneys
    {

        public static void ReNewArrivalAndDepartures(IConnection connection, ILogger logger)
        {
            string exchangeName = "FlightJourney";
            string routingKey = "WorkerFIDS.RenewData";
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "Dlx-exchange", type: ExchangeType.Direct);
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            var message = "renew";
            var property = channel.CreateBasicProperties();
            property.CorrelationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey,
                                     basicProperties: property,
                                     body: body);
            
            logger.LogInformation("Sent message from WorkerFIDS: " + message);
        }

        public static void GetArrivals(Guid client, WatsonWsServer server, IDatabase database)
        {
            var content = database.StringGet("Arrival");
            if (String.IsNullOrEmpty(content))
            {
                return;
            }
            var allCurrentArrivalsInCache = JsonConvert.DeserializeObject<List<FlightFIDSArrivalDTO>>(content);
            server.SendAsync(client, JsonConvert.SerializeObject(allCurrentArrivalsInCache.OrderByDescending(x => x.ArrivalDate)));
        }

        public static void GetDepartures(Guid client, WatsonWsServer server, IDatabase database)
        {
            var content = database.StringGet("Departs");
            if (String.IsNullOrEmpty(content))
            {
                return;
            }

            var allCurrentdeparturessInCache = JsonConvert.DeserializeObject<List<FlightFIDSDepartDTO>>(content);
            server.SendAsync(client, JsonConvert.SerializeObject(allCurrentdeparturessInCache.OrderByDescending(x => x.DepartureTime)));
        }


    }
}
