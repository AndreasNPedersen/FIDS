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
    public static class EnrichFJDepart
    {
        public static void SendFJDepartMessage(Flight flight, Airplane plane, IModel channel)
        {
            string exchangeName = "FlightJourney";
            string enrichedRoutingKey = "AirplanePlusJourney.Departure"; //to FIDS worker
            FlightFIDSDepartDTO enrichedData = new FlightFIDSDepartDTO
            {
                AirplaneOwner = plane.Owner,
                ToAirport = flight.ToLocation,
                DepartureTime = flight.DepartureDate,
                FlightJourneyId = flight.Id,
                Gate = 0, //this will change when Boarding updates it
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
