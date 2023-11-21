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
    public static class EnrichFJBooking
    {
        public static void SendFJMessage(Flight flight, string airplaneURL, IModel channel)
        {
            string exchangeName = "FlightJourney";
            string enrichedRoutingKey = "BookingPlaneAndFlight"; //to Boarding

            Airplane plane = HttpAirplane.GetAirplane(airplaneURL, flight.FlightId);

            FlightBookingDTO enrichedData = new FlightBookingDTO
            {
                Arrival = flight.ArrivalDate.ToString(),
                Departure = flight.DepartureDate.ToString(),
                FlightOrigin = flight.FromLocation,
                FlightDestination = flight.ToLocation,
                FlightId = flight.Id.ToString(),
                BaggageWeightAvailableTotal = Convert.ToInt32(plane.MaxWeightCargo),
                PassengersAvailableTotal = plane.MaxSeats,
                planeId = plane.Id.ToString(),
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
