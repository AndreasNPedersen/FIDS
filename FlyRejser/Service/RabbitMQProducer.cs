using FlyRejser.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace FlyRejser.Service
{
    public static class RabbitMQProducer
    {
        public static bool SendJourney(IConnection connection, ILogger logger, Travel travel)
        {
            string exchangeName = "FlightJourney";
            string[] routingKey = { "Journey.Created.FIDS.Departure", "Journey.Created.Boarding", "Journey.Created.FIDS.Arrival" };
            try
            {
                using var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "Dlx-exchange", type: ExchangeType.Direct);
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                var message = JsonConvert.SerializeObject(travel);
                var property = channel.CreateBasicProperties();
                property.CorrelationId = Guid.NewGuid().ToString();
                var body = Encoding.UTF8.GetBytes(message);
                foreach(var key in routingKey)
                {
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: key,
                                         basicProperties: property,
                                         body: body);
                }
                logger.LogInformation("Sent message from FlyRejser: " + message);
                return true;

            } catch (Exception ex)
            {
                logger.LogError("Error sending a FlyJourney to the rabbitmq in " + nameof(RabbitMQProducer));
                return false;
            }
        }

        public static bool SendCurrentJourney(IConnection connection, ILogger logger, Travel travel, string routingKey)
        {
            string exchangeName = "FlightJourney";
            try
            {
                using var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "Dlx-exchange", type: ExchangeType.Direct);
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                var message = JsonConvert.SerializeObject(travel);
                var property = channel.CreateBasicProperties();
                property.CorrelationId = Guid.NewGuid().ToString();
                var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: routingKey,
                                         basicProperties: property,
                                         body: body);
         
                logger.LogInformation("Sent message from FlyRejser: " + message);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error sending a FlyJourney to the rabbitmq in " + nameof(RabbitMQProducer));
                return false;
            }
        }

        public static bool SendUpdatedJourney(IConnection connection, ILogger logger, Travel travel)
        {
            string exchangeName = "FlightJourney";
            string[] routingKey = { "Journey.Updated.Booking", "Journey.Updated.FIDS.Departure", "Journey.Updated.FIDS.Arrival", "Journey.Updated.Boarding" };
            try
            {
                using var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "Dlx-exchange", type: ExchangeType.Direct);
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                var message = JsonConvert.SerializeObject(travel);
                var property = channel.CreateBasicProperties();
                property.CorrelationId = Guid.NewGuid().ToString();
                var body = Encoding.UTF8.GetBytes(message);
                foreach (var key in routingKey)
                {
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: key,
                                         basicProperties: property,
                                         body: body);
                }
                logger.LogInformation("Sent message from FlyRejser updated: " + message);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error sending a FlyJourney to the rabbitmq in " + nameof(RabbitMQProducer));
                return false;
            }
        }
    }
}
