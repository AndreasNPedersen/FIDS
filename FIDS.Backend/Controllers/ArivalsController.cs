using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FIDS.Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ArivalsController : ControllerBase
    {
        private readonly ILogger<DeparturesController> _logger;

        public ArivalsController(ILogger<DeparturesController> logger)
        {
            _logger = logger;
        }

        // GET: api/Travels
        [HttpGet]
        [HttpGet(Name = "GetAllDepartures")]
        public async Task<IEnumerable<TravelResponseDTO>> GetTravel()
        {
            _logger.LogInformation("GetAllDepartures");
            List<TravelResponseDTO> list = new List<TravelResponseDTO>();
            list.Add(new TravelResponseDTO(1, "Billund", "Berlin", DateTime.Now.AddHours(2), DateTime.Now, 7));
            return list;
        }
    }
}

