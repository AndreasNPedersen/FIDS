using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Models.DTO
{
    public class FlightBookingDTO
    {
        public string FlightId { get; set; }
        public string planeId { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string FlightOrigin { get; set; }
        public string FlightDestination { get; set; }
        public int PassengersAvailableTotal { get; set; }
        public double BaggageWeightAvailableTotal { get; set; }
    }
}
