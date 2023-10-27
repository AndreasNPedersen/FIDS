namespace FlyRejser.DTO
{
    public class TravelResponseDTO
    {
        public Guid Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid FlightId { get; set; }
        public string Status {  get; set; }
        public TravelResponseDTO(Guid id, string toLocation, string fromLocation, DateTime arrivalDate, DateTime departureDate, Guid flightId, string status)
        {
            Id = id;
            ToLocation = toLocation;
            FromLocation = fromLocation;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            FlightId = flightId;
            Status = status;
        }
    }
}
