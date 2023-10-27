using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System.Text;
using WatsonWebsocket;
using WorkerFIDS.Models;
using WorkerFIDS.Models.Gate;
using WorkerFIDS.Services;

namespace WorkerFIDS.Services
{
    //Not implemented yet from Baggage team
    public interface IBaggageConsumeMessaging
    {
        public void GetMessageGateIdChange();
        public void GetMessageGateStatus();

    }
    public class BaggageConsumeMessaging : IBaggageConsumeMessaging
    {
        private IConnection _connection;
        private ILogger _logger;
        private IDatabase _database;
        private WatsonWsServer _server;
        private List<Guid> _clients;

        public BaggageConsumeMessaging(IConnection connection, ILogger logger, IDatabase database, WatsonWsServer server, List<Guid> clients)
        {
            _connection = connection;
            _logger = logger;
            _server = server;
            _database = database;
            _clients = clients;
        }

        public void GetMessageGateIdChange()
        {
            string queueName = "FlightJourneyGate";
            string[] exchangeNames = { "BoardingService.Infrastructure.Gate.Consumers.GateAssigned", 
                "BoardingService.Infrastructure.Gate.Consumers.GateUpdated" };
            string routingKey = string.Empty;

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" }, { "x-dead-letter-routing-key", "FlyJourneyEnrich" }, 
                { "x-message-ttl", 900000 } });
            foreach (var exchange in exchangeNames)
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
                channel.QueueBind(queue: queueName,
                              exchange: exchange,
                              routingKey: routingKey);
            }
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    string departures = _database.StringGet("Departs");
                    List<FlightFIDSDepartDTO> flightDeps = String.IsNullOrEmpty(departures) ?
                        JsonConvert.DeserializeObject<List<FlightFIDSDepartDTO>>(departures) :
                        null;
                    if (flightDeps == null)
                    {
                        throw new Exception();
                    }
                    var gate = JsonConvert.DeserializeObject<GateUpdateDTO>(message);
                    var flight = flightDeps.Find(x => x.FlightJourneyId == gate.FlightJourneyId);
                    var flightId = flightDeps.IndexOf(flight);
                    flightDeps[flightId].Gate = gate.GateId;
                    _database.StringSet("Departs", JsonConvert.SerializeObject(flightDeps));
                    WebSocketFIDS.SendMessageToFrontendDeparture(flightDeps, _server, _clients);
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError("Couldn't find a FlightJourney or bad deserialization: " + ex.Message);
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            _logger.LogInformation("started " + nameof(GateConsumeMessaging));
            Console.ReadLine();
        }
        public void GetMessageGateStatus()
        {
            string queueName = "FlightJourneyGate";
            string exchangeName = "BoardingService.Infrastructure.Gate.Consumers.CheckGate";
            string routingKey = string.Empty;

            using var channel = _connection.CreateModel();
            // declare a server-named queue
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" },
                { "x-dead-letter-routing-key", "FlyJourneyEnrich" }, { "x-message-ttl", 900000 } });

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    string departures = _database.StringGet("Departs");
                    List<FlightFIDSDepartDTO> flightDeps = String.IsNullOrEmpty(departures) ?
                        JsonConvert.DeserializeObject<List<FlightFIDSDepartDTO>>(departures) :
                        null;
                    if (flightDeps == null)
                    {
                        throw new Exception();
                    }
                    var gate = JsonConvert.DeserializeObject<GateUpdateStatusDTO>(message);
                    var flight = flightDeps.Find(x => x.FlightJourneyId == gate.FlightJourneyId);
                    var flightId = flightDeps.IndexOf(flight);
                    flightDeps[flightId].Status = gate.Status;
                    _database.StringSet("Departs", JsonConvert.SerializeObject(flightDeps));
                    WebSocketFIDS.SendMessageToFrontendDeparture(flightDeps, _server, _clients);
                } catch (Exception ex)
                {
                    _logger.LogError("Couldn't find a FlightJourney or bad deserialization: " + ex.Message);
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            _logger.LogInformation("started " + nameof(GateConsumeMessaging));
            Console.ReadLine();
        }
    }
}

