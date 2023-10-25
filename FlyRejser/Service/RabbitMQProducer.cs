using FlyRejser.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace FlyRejser.Service
{
    public static class RabbitMQProducer
    {
        public static void SendJourney(IConnection connection, ILogger logger, Travel travel)
        {
            string exchangeName = "FlightJourney";
            string[] routingKey = { "Journey.Created.Booking", "Journey.Created.FIDS" };
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
        }

        public static void SendUpdatedJourney(IConnection connection, ILogger logger, Travel travel)
        {
            string exchangeName = "FlightJourney";
            string[] routingKey = { "Journey.Updated.Booking", "Journey.Updated.FIDS" };
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
        }
    }
}
