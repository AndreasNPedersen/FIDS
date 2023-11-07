using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerFIDS.Models
{
    public class ClientTypes
    {
        public List<Guid> DepartureClients { get; set; }
        public List<Guid> ArrivalClients { get; set; }
    }
}
