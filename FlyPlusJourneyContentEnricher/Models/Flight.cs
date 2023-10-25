using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int FlightId { get; set; }
        public string Status { get; set; }
    }
}
