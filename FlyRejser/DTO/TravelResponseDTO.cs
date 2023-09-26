namespace FlyRejser.DTO
{
    public class TravelResponseDTO
    {
        public int Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int FlightId { get; set; }
        public TravelResponseDTO(int id, string toLocation, string fromLocation, DateTime arrivalDate, DateTime departureDate, int flightId)
        {
            Id = id;
            ToLocation = toLocation;
            FromLocation = fromLocation;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            FlightId = flightId;
        }
    }
}
