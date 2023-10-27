using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerFIDS.Models
{
    public class FlightFIDSDepartDTO
    {
        public DateTime DepartureTime { get; set; }
        public string ToAirport { get; set; }
        public string AirplaneOwner { get; set; }
        public Guid FlightJourneyId { get; set; }
        public string Status { get; set; }
        public int Gate { get; set; }
    }
}
