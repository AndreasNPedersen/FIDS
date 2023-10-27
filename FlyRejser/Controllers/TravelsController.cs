using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlyRejser.Data;
using FlyRejser.DTO;
using System.Collections.Generic;
using FlyRejser.Service;
using RabbitMQ.Client;
using System.Diagnostics;

namespace FlyRejser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : ControllerBase
    {
        private readonly FlyRejserContext _context;
        private readonly ILogger<TravelsController> _logger;
        private FlightServiceAPIClient _flightService;
        private IConnection _connection;

        public TravelsController(FlyRejserContext context, ILogger<TravelsController> log)
        {
            _context = context;
            _logger = log;
            _flightService = new FlightServiceAPIClient(_logger);
            var factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitIP") };
            _connection = factory.CreateConnection();
        }

        // GET: api/Travels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelResponseDTO>>> GetTravels()
        {
          if (_context.Travel == null)
          {
              return NotFound();
          }
            return await _context.Travel.Select(x => new TravelResponseDTO(x.Id, x.ToLocation, x.FromLocation, x.ArrivalDate, x.DepartureDate, x.FlightId, x.Status)).ToListAsync();
        }
       

        // GET: api/Travels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelResponseDTO>> GetTravel(Guid id)
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

            var responseDto = new TravelResponseDTO(travel.Id, travel.ToLocation, travel.FromLocation, travel.ArrivalDate, travel.DepartureDate, travel.FlightId, travel.Status);
            
            return responseDto;
        }

        // PATCH: api/Travels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTravel(Guid id, TravelRequestUpdateDTO travelUpdateDTO)
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
            if (travelUpdateDTO.ArrivalDate !< travelUpdateDTO.DepartureDate)
            {
                travel.ArrivalDate = travelUpdateDTO.ArrivalDate;
                travel.DepartureDate = travelUpdateDTO.DepartureDate;
            }
            travel.Status = travelUpdateDTO.Status;
            travel.FlightId = travelUpdateDTO.FlightId;


            _context.Entry(travel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                RabbitMQProducer.SendUpdatedJourney(_connection, _logger, travel);
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

        // PATCH: api/Travels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchTravelStatus(Guid id, [FromBody] string status)
        {

            var travel = await _context.Travel.FindAsync(_context.Travel.Find(id));
            
            if (travel == null)
            {
                return BadRequest();
            }

            travel.Status = status;


            _context.Entry(travel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                RabbitMQProducer.SendUpdatedJourney(_connection, _logger, travel);
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
            
            var flight = await  _flightService.GetFlightIdAsync(travelRequest.FlightId);
            if (_context.Travel == null && flight == null)
            {
                return Problem("Entity set 'FlyRejserContext.Travel'  is null.");
            }

            var travel = new Travel(travelRequest.ToLocation, travelRequest.FromLocation, travelRequest.ArrivalDate, travelRequest.DepartureDate,flight.Id,"On Time");

            _context.Travel.Add(travel);
            
            await _context.SaveChangesAsync();
            RabbitMQProducer.SendJourney(_connection,_logger,travel); 
            return CreatedAtAction("GetTravel", new { id = travel.Id }, travel);
        }

        // DELETE: api/Travels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravel(Guid id)
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

        private bool TravelExists(Guid id)
        {
            return (_context.Travel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
