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
        _logger.LogInformation("Klient henter Ankomst iniformationer....");
        List<TravelResponseDTO> list = new List<TravelResponseDTO>();
        var item = new TravelResponseDTO(1, "Billund", "Berlin", DateTime.Now.AddHours(2), DateTime.Now, 7);
        new TravelResponseDTO(1, "Tuzla", "Billund", DateTime.Now.AddHours(2), DateTime.Now, 7);
        _logger.LogInformation("Klient har hentet informationer om afgang:", item.ToString());
        list.Add(item);
        return list;
    }
}
