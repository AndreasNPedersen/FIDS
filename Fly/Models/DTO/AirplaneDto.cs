namespace Fly.Models.DTO
{
    public class AirplaneDto
    {
        public string Type { get; set; }
        public string Owner { get; set; }
        public double MaxWeightCargo { get; set; }
        public double MaxVolumeCargo { get; set; }
        public int MaxSeats { get; set; }
    }
}
