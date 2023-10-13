using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlyRejser.Data;
using FlyRejser.DTO;
using System.Collections.Generic;
using FlyRejser.Service;

namespace FlyRejser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : ControllerBase
    {
        private readonly FlyRejserContext _context;
        private readonly ILogger _logger;
        private FlightServiceAPIClient _flightService;

        public TravelsController(FlyRejserContext context, ILogger log)
        {
            _context = context;
            _logger = log;
            _flightService = new FlightServiceAPIClient(_logger);
        }

        // GET: api/Travels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelResponseDTO>>> GetTravels()
        {
          if (_context.Travel == null)
          {
              return NotFound();
          }
            return await _context.Travel.Select(x => new TravelResponseDTO(x.Id, x.ToLocation, x.FromLocation, x.ArrivalDate, x.DepartureDate, x.FlightId)).ToListAsync();
        }

        // GET: api/Travels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelResponseDTO>> GetTravel(int id)
        {
            if (_context.Travel == null)
            {
                return NotFound();
            }
            var travel = await _context.Travel.FindAsync(id);

            if (travel == null)
            {
                return NotFound();
            }

            var responseDto = new TravelResponseDTO(travel.Id, travel.ToLocation, travel.FromLocation, travel.ArrivalDate, travel.DepartureDate, travel.FlightId);
            
            return responseDto;
        }

        // PATCH: api/Travels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTravel(int id, TravelRequestUpdateDTO travelUpdateDTO)
        {
            if (id != travelUpdateDTO.Id)
            {
                return BadRequest();
            }

            var travel = await _context.Travel.FindAsync(_context.Travel.Find(travelUpdateDTO.Id));
            if (travel.ToLocation != travelUpdateDTO.ToLocation)
            {
                travel.ToLocation = travelUpdateDTO.ToLocation;
            }
            if (travelUpdateDTO.FromLocation != travelUpdateDTO.FromLocation)
            {
                travel.FromLocation = travelUpdateDTO.FromLocation;
            }

            _context.Entry(travel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Travels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Travel>> PostTravel(TravelRequestDTO travelRequest)
        {
            
            var flight = _flightService.GetFlightIdAsync(travelRequest.FlightId);
            if (_context.Travel == null && flight == null)
            {
                return Problem("Entity set 'FlyRejserContext.Travel'  is null.");
            }

            var travel = new Travel(travelRequest.ToLocation, travelRequest.FromLocation, travelRequest.ArrivalDate, travelRequest.DepartureDate,flight.Id);

            _context.Travel.Add(travel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravel", new { id = travel.Id }, travel);
        }

        // DELETE: api/Travels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravel(int id)
        {
            if (_context.Travel == null)
            {
                return NotFound();
            }
            var travel = await _context.Travel.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }

            _context.Travel.Remove(travel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelExists(int id)
        {
            return (_context.Travel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
