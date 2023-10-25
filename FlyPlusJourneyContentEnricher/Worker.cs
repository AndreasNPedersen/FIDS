using RabbitMQ.Client;

namespace FlyPlusJourneyContentEnricher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FlyJourneyEnrich enricherRabbit;
        public Worker(ILogger<Worker> logger)
        {
            string airplaneURL = "http://Fly/Airplane/";
            _logger = logger;
            var factory = new ConnectionFactory { HostName = "host.docker.internal" };
            enricherRabbit = new FlyJourneyEnrich(factory, _logger,airplaneURL);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            enricherRabbit.GetMessage();
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }
    }
}