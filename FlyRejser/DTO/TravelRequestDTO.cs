namespace FlyRejser.DTO
{
    public class TravelRequestDTO
    {
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
