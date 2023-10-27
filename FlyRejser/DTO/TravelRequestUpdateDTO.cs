namespace FlyRejser.DTO
{
    public class TravelRequestUpdateDTO
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
