using FlyRejser.Data;
using FlyRejser.Service;
using RabbitMQ.Client;

namespace FlyRejser.Worker
{
    public class WorkerGateUpdates : BackgroundService
    {
        private IConnection _connection;
        private readonly ILogger<WorkerGateUpdates> _logger;
        private FlyRejserContext _context;
        public WorkerGateUpdates(ILogger<WorkerGateUpdates> logger,FlyRejserContext context) {
            _logger = logger;
            var factory = new ConnectionFactory { HostName = "host.docker.internal" };
            _connection = factory.CreateConnection();
            _context = context;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            GateConsumeMessaging gateConsumeMessaging = new GateConsumeMessaging(_connection,_logger,_context);
            gateConsumeMessaging.GetMessageGateStatus();

        }
    }
}
