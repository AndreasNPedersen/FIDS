using FlyPlusJourneyContentEnricher.Models;
using FlyPlusJourneyContentEnricher.Models.DTO;
using FlyPlusJourneyContentEnricher.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher
{
    public interface IFlyJourneyEnrich
    {
        public void GetMessage();
    }
    public class FlyJourneyEnrich
    {
        private readonly string exchangeName = "FlightJourney";
        private ConnectionFactory _factory;
        private readonly ILogger _logger;
        private readonly string _airplaneURL;

        public FlyJourneyEnrich(ConnectionFactory factory, ILogger logger, string airplaneURL) {
            _logger = logger;
            _factory = factory;
            _airplaneURL = airplaneURL;
        }
        public void GetMessage()
        {
            string queueName = "FIDSEnricher";
            string routingKey = "Journey.*.FIDS.*"; //From FlyRejser

            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            channel.QueueDeclare(queueName, true, false, false,
                new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" },
            { "x-dead-letter-routing-key", "FlyJourneyEnrich"}, { "x-message-ttl", 900000 }}); 
              channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: routingKey);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    Flight flight = JsonConvert.DeserializeObject<Flight>(message);
                    
                    _logger.LogInformation(message);
                    string key = ea.RoutingKey;
                    switch (key) //should be created as a strategy pattern
                    {
                        case string a when a.Contains("Departure"):
                            EnrichFJDepart.SendFJDepartMessage(flight, _airplaneURL, channel);
                            break;
                        case string b when b.Contains("Arrival"):
                            EnrichFJBooking.SendFJMessage(flight, _airplaneURL, channel);
                            EnrichFJArrival.SendFJArrivalMessage(flight, _airplaneURL, channel);
                            break;
                    }
                } catch(Exception ex) { _logger.LogError(ex.Message); }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            _logger.LogInformation("started " + nameof(FlyJourneyEnrich));
            Console.ReadLine();
        }
    }
}
