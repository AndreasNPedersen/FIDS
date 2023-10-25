using FlyPlusJourneyContentEnricher.Models;
using FlyPlusJourneyContentEnricher.Models.DTO;
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
            string[] routingKey = { "Journey.Created.FIDS", "Journey.Updated.FIDS" }; //From FlyRejser
            string enrichedRoutingKey = "AirplanePlusJourney"; //to FIDS worker

            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            channel.QueueDeclare(queueName, true, false, false); //new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" },
            //{ "x-dead-letter-routing-key", "FlyJourneyEnrich"}, { "x-message-ttl", 900000 }}

            foreach (var key in routingKey)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: key);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    Flight flight = JsonConvert.DeserializeObject<Flight>(message);
                    var response = new HttpClient().GetAsync(_airplaneURL + flight.FlightId).Result;
                    var responseStringify = response.Content.ReadAsStringAsync().Result;
                    Airplane plane = JsonConvert.DeserializeObject<Airplane>(responseStringify);
                    _logger.LogInformation(message);

                    FlightFIDSDepartDTO enrichedData = new FlightFIDSDepartDTO
                    {
                        AirplaneOwner = plane.Owner,
                        ToAirport = flight.ToLocation,
                        DepartureTime = flight.DepartureDate,
                        FlightJourneyId = flight.Id.ToString(),
                        Gate = 1, //missing api call
                        Status = flight.Status 
                    };

                    //send enriched data
                    var enrichedDataMessage = JsonConvert.SerializeObject(enrichedData);
                    var property = channel.CreateBasicProperties();
                    property.CorrelationId = Guid.NewGuid().ToString();
                    var enrichedDatabody = Encoding.UTF8.GetBytes(enrichedDataMessage);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: enrichedRoutingKey,
                                         basicProperties: property,
                                         body: enrichedDatabody);

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
