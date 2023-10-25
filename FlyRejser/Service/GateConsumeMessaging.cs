using FlyRejser.Data;
using FlyRejser.DTO.Gate;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FlyRejser.Service
{
    //update gaten ved i databasen og derfra send med RabbitMQProducer updated
    public class GateConsumeMessaging
    {
        private IConnection _connection;
        private ILogger _logger;
        private FlyRejserContext _context;

        public GateConsumeMessaging(IConnection connection, ILogger logger, FlyRejserContext dbContext)
        {
            _connection = connection;
            _logger = logger;
            _context = dbContext;
        }

        public void GetMessageGateIdChange()
        {
            string queueName = "FlightJourneyGate";
            string exchangeName = "FlightJourney";
            string routingKey = "Journey.Updated.Gate";

            using var channel = _connection.CreateModel();
            // declare a server-named queue
            channel.QueueDeclare(queueName, true, false, false);

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
                    var gate = JsonConvert.DeserializeObject<GateUpdateDTO>(message);
                    var travel = _context.Travel.Find(gate.FlightJourneyId);
                    if (travel != null & gate != null)
                    {
                        _context.Travel.Update(travel);
                        RabbitMQProducer.SendUpdatedJourney(_connection, _logger, travel);
                    }
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
            string exchangeName = "FlightJourney";
            string routingKey = "Journey.Updated.Gate.Status";

            using var channel = _connection.CreateModel();
            // declare a server-named queue
            channel.QueueDeclare(queueName, true, false, false);

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
                    var gate = JsonConvert.DeserializeObject<GateUpdateStatusDTO>(message);
                    var travel = _context.Travel.Find(gate.FlightJourneyId);
                    if (travel != null & gate !=null)
                    {
                        travel.Status = gate.Status;
                        _context.Travel.Update(travel);
                        RabbitMQProducer.SendUpdatedJourney(_connection, _logger, travel);
                    }
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

