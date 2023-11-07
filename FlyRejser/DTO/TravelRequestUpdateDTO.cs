namespace FlyRejser.DTO
{
    public class TravelRequestUpdateDTO
    {
        public Guid Id { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid FlightId { get; set; }
        public string Status { get; set; }
    }
}
