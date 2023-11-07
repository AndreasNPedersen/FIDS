

using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WatsonWebsocket;
using WorkerFIDS.Models;
using WorkerFIDS.Services;

namespace WorkerFIDS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RabbitFlightPlusJourney _flightPlusJourney;
        private ConnectionFactory _connectionFactory;
        private WatsonWsServer _server;
        private ConnectionMultiplexer _redis;
        private GateConsumeMessaging _mq;
        private ClientTypes _clientTypes;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitIP") }; //host.docker.internal
            _server = new WatsonWsServer("*", 8010);
            _redis = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisIP"));
            _clientTypes = new ClientTypes();
            _clientTypes.ArrivalClients = new List<Guid>();
            _clientTypes.DepartureClients = new List<Guid>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = _connectionFactory.CreateConnection();
            _server.Start();
            var database = _redis.GetDatabase();
            if (!database.KeyExists("Arrival") && !database.KeyExists("Departs"))
            {
                GetCurrentFlightJourneys.ReNewArrivalAndDepartures(connection,_logger);
            }
            _flightPlusJourney = new RabbitFlightPlusJourney(connection, _logger, _server, database, _clientTypes);
            _mq = new GateConsumeMessaging(connection, _logger, database, _server, _clientTypes.DepartureClients);
            //Rabbitmq messages
            Task.Run(() =>
            {
                _flightPlusJourney.ReceiveMessage();
            });

            Task.Run(() =>
            {
                _mq.GetMessageGateIdChange();
            });
 
            Task.Run(() =>
            {
            _mq.GetMessageGateStatus();

            });

            Task.Run(() =>
            {
                StartListeningOnClientsFrontend(database);
            });

                Console.ReadLine();
        }
        public async Task StartListeningOnClientsFrontend(IDatabase database)
        {
            _clientTypes.ArrivalClients = new List<Guid>();
            _clientTypes.DepartureClients = new List<Guid>();
            _server.MessageReceived += (sender, e) =>
            {
                if (_clientTypes.DepartureClients.Contains(e.Client.Guid) ||
                _clientTypes.ArrivalClients.Contains(e.Client.Guid)) { return; }
                byte[] body = e.Data.ToArray();
                var message = Encoding.UTF8.GetString(body);
                switch (message)
                {
                    case "Departure":
                        _clientTypes.DepartureClients.Add(e.Client.Guid);
                        GetCurrentFlightJourneys.GetDepartures(e.Client.Guid, _server, database);
                        break;
                    case "Arrival":
                        GetCurrentFlightJourneys.GetArrivals(e.Client.Guid,_server,database);
                        _clientTypes.ArrivalClients.Add(e.Client.Guid);
                        break;
                }
            };
            _server.ClientDisconnected += (sender, e) =>
            {
                _clientTypes.ArrivalClients.Remove(e.Client.Guid);
                _clientTypes.DepartureClients.Remove(e.Client.Guid);
            };
            Console.ReadLine();
        }
    }
}