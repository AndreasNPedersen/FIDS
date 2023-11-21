using FlyPlusJourneyContentEnricher.Helpers;
using FlyPlusJourneyContentEnricher.Models;
using FlyPlusJourneyContentEnricher.Models.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Services
{
    public static class EnrichFJArrival
    {
        public static void SendFJArrivalMessage(Flight flight, string airplaneURL, IModel channel)
        {
            string exchangeName = "FlightJourney";
            string enrichedRoutingKey = "AirplanePlusJourney.Arrival"; //to FIDS worker


            Airplane plane = HttpAirplane.GetAirplane(airplaneURL, flight.FlightId);

            FlightFIDSArrivalDTO enrichedData = new FlightFIDSArrivalDTO
            {
                AirplaneOwner = plane.Owner,
                FromLocation = flight.FromLocation,
                ArrivalDate = flight.ArrivalDate,
                FlightJourneyId = flight.Id,
                BagageClaimGate = "1", //missing api call
                Status = flight.Status
            };

            //send enriched data
            var enrichedDataMessage = JsonConvert.SerializeObject(enrichedData);
            var property = channel.CreateBasicProperties();
            property.CorrelationId = Guid.NewGuid().ToString();
            var enrichedDatabody = Encoding.UTF8.GetBytes(enrichedDataMessage);
            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: enrichedRoutingKey,
                                 basicProperties: property,
                                 body: enrichedDatabody);
        }
    }
}
