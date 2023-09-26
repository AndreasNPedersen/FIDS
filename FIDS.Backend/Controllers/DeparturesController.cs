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
    [ProducesResponseType(typeof(IEnumerable<Departures>), 200)]
    public IActionResult GetAllDepartures()
    {
        _logger.LogInformation("GetAllDepartures");
        return Ok();
    }
}

public class Departures { }
