namespace FlyRejser.Data
{
    public class Travel
    {
        public int Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int FlightId { get; set; }
        public string Status { get; set; }
        public Travel()
        {

        }
        public Travel(string toLocation, string fromLocation, DateTime arrivalDate, DateTime departureDate, int flightId, string status)
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
