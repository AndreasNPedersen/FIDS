

using Fleck;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkerFIDS.Models;


namespace WorkerFIDS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private RabbitFlightPlusJourneyDepart flightPlusJourneyDepart;
        private ConnectionFactory _connectionFactory;
        private WebSocketServer _server;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory { HostName = "host.docker.internal" }; //host.docker.internal
            _server = new WebSocketServer("ws://0.0.0.0:8010");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var connection = _connectionFactory.CreateConnection();
            _server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => socket.Send(message);
            });
            //flightPlusJourneyDepart = new RabbitFlightPlusJourneyDepart(connection, _logger, _server);
            //flightPlusJourneyDepart.ReceiveMessage();

            while (!stoppingToken.IsCancellationRequested)
            {               
            }
        }
    }
}