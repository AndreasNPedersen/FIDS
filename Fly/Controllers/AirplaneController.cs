using Fly.Models.DTO;
using Fly.Models.Entities;
using Fly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;

namespace Fly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirplaneController : ControllerBase
    {
        private IAirplaneService _airplaneService;
        public AirplaneController(IAirplaneService airplaneService) {
            _airplaneService = airplaneService;
        }

        // GET: Airplane
        [HttpGet]
        public async Task<ActionResult<List<Airplane>>> GetAll()
        {
            return Ok( await _airplaneService.GetAllAirplaneAsync());
        }

        // GET: Airplane/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Airplane>> GetOne(Guid id)
        {
            Airplane plane = await _airplaneService.GetAirplaneByIdAsync(id);
            if (plane == null)
            {
                return NotFound();
            }
            return Ok(plane);
            
        }

        // POST: Airplane
        [HttpPost]
        public async Task<ActionResult> Create(AirplaneDto airplane)
        {
            bool created = await _airplaneService.AddAirplaneAsync(airplane);
            switch (created)
            {
                case true:
                    return Ok();
                case false:
                    return BadRequest();
            };
        }

        // DELETE: Airplane/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _airplaneService.DeleteAirplaneAsync(id);
            switch (deleted)
            {
                case true:
                    return Ok();
                case false:
                    return BadRequest();
            };
        }

       
    }
}
