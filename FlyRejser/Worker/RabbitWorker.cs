using FlyRejser.Data;
using FlyRejser.Service;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace FlyRejser.Worker
{
    public class RabbitWorker : BackgroundService
    {
        private readonly ILogger<RabbitWorker> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        private IConnection _connection;
        private WorkerFIDSRabbitConsumer _consumer;

        public RabbitWorker(ILogger<RabbitWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            var connectionFactory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitIP") };
            _connection = connectionFactory.CreateConnection();
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

                Task.Run(() => { 
            using (var scope = scopeFactory.CreateScope())
            {
                    var dbContext = scope.ServiceProvider.GetRequiredService<FlyRejserContext>();
                    _consumer = new WorkerFIDSRabbitConsumer(_connection, _logger, dbContext);
                    _consumer.GetMessage();
            }
                });
        }
    }
}
