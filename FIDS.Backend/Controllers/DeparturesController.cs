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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetAllDepartures")]
    [ProducesResponseType(typeof(IEnumerable<Flight>), 200)]
    public IEnumerable<Flight> GetFlights()
    {
        _logger.LogInformation("GetAllDepartures");
        return new List<Flight>
        {
            new Flight { Id = 2, Destination = "Copenhagen", Status = "On Time" },
            // Tilføj flere flights som nødvendigt
        };
    }
}

public class Departures { }
