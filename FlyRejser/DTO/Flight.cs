namespace FlyRejser.DTO
{
    public class Flight
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public double MaxWeightCargo { get; set; }
        public double MaxVolumeCargo { get; set; }
        public int MaxSeats { get; set; }
    }
}
