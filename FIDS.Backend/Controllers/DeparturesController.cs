using System.Data.SqlTypes;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FIDS.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class DeparturesController : ControllerBase
{
    private readonly ILogger<DeparturesController> _logger;

    public DeparturesController(ILogger<DeparturesController> logger)
    {
        _logger = logger;
    }

    // GET: api/Travels
    [HttpGet(Name = "GetAllDepartures")]
    public async Task<IEnumerable<TravelResponseDTO>> GetDepartures()
    {
        _logger.LogInformation("GetAllDepartures");
        List<TravelResponseDTO> list = new List<TravelResponseDTO>();
        list.Add(new TravelResponseDTO(1, "Madridd", "Billund", DateTime.Now.AddHours(2), DateTime.Now, 7));
        return list;
    }
}
