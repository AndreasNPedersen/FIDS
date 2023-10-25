//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Channels;
//using System.Threading.Tasks;
//using WatsonWebsocket;
//using WorkerFIDS.Models;

//namespace WorkerFIDS.Services
//{
//    public class RabbitFlightPlusJourneyDepart
//    {
//        private IConnection _connection;
//        private ILogger _logger;
//        private List<FlightFIDSDepartDTO> _flightDeps;
//        private WatsonWsServer _server;
//        public RabbitFlightPlusJourneyDepart(IConnection connection, ILogger logger, WatsonWsServer server) { 
//            _connection = connection;
//            _logger = logger;
//            _flightDeps = new List<FlightFIDSDepartDTO>();
//            _server = server;
//        }

       

//        public void ReceiveMessage()
//        {
//            string queueName = "FIDSEnricher";
//            string exchangeName = "FlightJourney";
//            string enrichedRoutingKey = "AirplanePlusJourney";

//            using var channel = _connection.CreateModel();
//            // declare a server-named queue
//            channel.QueueDeclare(queueName, true, false, false);

//            channel.QueueBind(queue: queueName,
//                          exchange: exchangeName,
//                          routingKey: enrichedRoutingKey);
//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += (model, ea) =>
//            {
//                byte[] body = ea.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);
//                try
//                {
//                    FlightFIDSDepartDTO fIDSDTO = JsonConvert.DeserializeObject<FlightFIDSDepartDTO>(message);
//                    _flightDeps.RemoveAll(x => x.DepartureTime.Day < DateTime.Now.Day - 1); //removes old flights from memory
//                    //lav if sætning der tager hvis der allerede findes så update eller add på listen
//                    var fJ = _flightDeps.Find(x => x.FlightJourneyId == fIDSDTO.FlightJourneyId);
//                    if (fJ != null && fJ.FlightJourneyId == fIDSDTO.FlightJourneyId)
//                    {
//                        _flightDeps[_flightDeps.IndexOf(fJ)] = fIDSDTO;
//                    } else
//                    {
//                        _flightDeps.Add(fIDSDTO);
//                    }
//                    WebSocketFIDS.SendMessageToFrontend(_flightDeps, _server);

//                } catch (Exception ex)
//                {
//                    _logger.LogError("Failed to deserialize object of: " + nameof(FlightFIDSDepartDTO) + ". " + ex.Message);
//                }
                
//            };
//            channel.BasicConsume(queue: queueName,
//                                 autoAck: true,
//                                 consumer: consumer);
//            _logger.LogInformation("started " + nameof(RabbitFlightPlusJourneyDepart));
//            Console.ReadLine();
//        }

//    }
//}
