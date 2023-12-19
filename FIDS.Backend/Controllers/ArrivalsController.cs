using FIDS.Backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FIDS.Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ArrivalsController : ControllerBase
    {
        private readonly ILogger<DeparturesController> _logger;

        public ArrivalsController(ILogger<DeparturesController> logger)
        {
            _logger = logger;
        }

        // GET: api/Travels
        [HttpGet(Name = "GetAllArrivals")]
        public async Task<IEnumerable<TravelResponseDTO>> GetArrivals()
        {
            _logger.LogInformation("GetAllArrivals-v2");
            List<TravelResponseDTO> list = new List<TravelResponseDTO>();
            list.Add(new TravelResponseDTO(1, "Billund", "Berlin", DateTime.Now.AddHours(2), DateTime.Now, 7));
            return list;
        }
    }
}

