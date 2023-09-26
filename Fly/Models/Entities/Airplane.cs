using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fly.Models.Entities
{
    public class Airplane
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        [MinLength(1)]
        public string Type { get; set; }
        [MaxLength(200)]
        [MinLength(1)]
        public string Owner { get; set; }
        public double MaxWeightCargo { get; set; }
        public double MaxVolumeCargo { get; set; }
        public int MaxSeats { get; set; }
    }
}
