using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public double MaxWeightCargo { get; set; }
        public double MaxVolumeCargo { get; set; }
        public int MaxSeats { get; set; }
    }
}
