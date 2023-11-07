using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using WatsonWebsocket;
using WorkerFIDS.Models;
using WorkerFIDS.Services.HowToLeave;

namespace WorkerFIDS.Services
{
    public interface IRabbitFlightPlusJourney
    {
        public Task ReceiveMessage();
    }
    public class RabbitFlightPlusJourney : IRabbitFlightPlusJourney
    {
        private IConnection _connection;
        private ILogger _logger;
        private List<FlightFIDSDepartDTO> _flightDeps;
        private WatsonWsServer _server;
        private IDatabase _redis;
        private ClientTypes _clientTypes;
        public RabbitFlightPlusJourney(IConnection connection, ILogger logger, WatsonWsServer server, IDatabase redis, ClientTypes clientTypes)
        {
            _connection = connection;
            _logger = logger;
            _flightDeps = new List<FlightFIDSDepartDTO>();
            _server = server;
            _redis = redis;
            _clientTypes = clientTypes;
        }

        

        public async Task ReceiveMessage()
        {
            string queueName = "FIDSEnriched";
            string exchangeName = "FlightJourney";
            string enrichedRoutingKey = "AirplanePlusJourney.*";

            using var channel = _connection.CreateModel();

            channel.QueueDeclare(queueName, true, false, false,
                new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" },
            { "x-dead-letter-routing-key", "FlyJourneyFIDS"}, { "x-message-ttl", 900000 }});

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: enrichedRoutingKey);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    switch (ea.RoutingKey) //make strategy pattern
                    {
                        case string a when a.Contains("Departure"):
                            DepartureService.SendEnrichedFJ(message, _redis, _server, _clientTypes.DepartureClients);
                            break;
                        case string b when b.Contains("Arrival"):
                            ArrivelService.SendEnrichedFJ(message, _redis, _server, _clientTypes.ArrivalClients);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Routing key is not in the switchcase in " + 
                                nameof(RabbitFlightPlusJourney));
                    }
         
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to deserialize object of: " + nameof(FlightFIDSDepartDTO) + ". " + ex.Message);
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            _logger.LogInformation("started " + nameof(RabbitFlightPlusJourney));
            Console.ReadLine();
        }

    }
}
