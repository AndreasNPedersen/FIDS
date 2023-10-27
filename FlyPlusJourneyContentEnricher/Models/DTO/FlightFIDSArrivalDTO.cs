using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Models.DTO
{
    public class FlightFIDSArrivalDTO
    {
        public string FlightJourneyId { get; set; }

        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string AirplaneOwner { get; set; }
        public string BagageClaimGate { get; set; }
        public string Status { get; set; }
    }
}
