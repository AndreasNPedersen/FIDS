using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlyRejser.Data
{
    public class Travel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid FlightId { get; set; }
        public string Status { get; set; }
        public Travel()
        {

        }
        public Travel(string toLocation, string fromLocation, DateTime arrivalDate, DateTime departureDate, Guid flightId, string status)
        {
            ToLocation = toLocation;
            FromLocation = fromLocation;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            FlightId = flightId;
            Status = status;
        }
    }
}
