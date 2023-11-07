using FlyRejser.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FlyRejser.Service
{
    public interface IWorkerFIDSRabbitConsumer
    {
        public void GetMessage();
    }
    public class WorkerFIDSRabbitConsumer : IWorkerFIDSRabbitConsumer
    {
        private IConnection _connection;
        private ILogger _logger;
        private FlyRejserContext _context;

        public WorkerFIDSRabbitConsumer(IConnection connection, ILogger logger, FlyRejserContext dbContext)
        {
            _connection = connection;
            _logger = logger;
            _context = dbContext;
        }

        public void GetMessage()
        {
            string queueName = "WorkerFIDS";
            string exchangeName = "FlightJourney";
            string routingKey = "WorkerFIDS.RenewData";

            using var channel = _connection.CreateModel();
            // declare a server-named queue
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object> { { "x-dead-letter-exchange", "Dlx-exchange" }, 
                { "x-dead-letter-routing-key", "FlyJourney" }, { "x-message-ttl", 900000 } });

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: routingKey);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var currentFjsDep = _context.Travel.Where(x => x.DepartureDate.Day == DateTime.Now.Day);
                foreach (var fj in currentFjsDep)
                {
                    RabbitMQProducer.SendCurrentJourney(_connection, _logger, fj, $"Journey.{message}.FIDS.Departure");
                }
                var currentFjsArr = _context.Travel.Where(x => x.ArrivalDate.Day == DateTime.Now.Day);
                foreach (var fj in currentFjsArr)
                {
                    RabbitMQProducer.SendCurrentJourney(_connection, _logger, fj, $"Journey.{message}.FIDS.Arrival");
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            _logger.LogInformation("started " + nameof(WorkerFIDSRabbitConsumer));
            Console.ReadLine();
        
        }
    }
}
