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
        public ActionResult<List<Airplane>> GetAll()
        {
            return Ok(_airplaneService.GetAllAirplane());
        }

        // GET: Airplane/5
        [HttpGet("{id}")]
        public ActionResult<Airplane> GetOne(int id)
        {
            Airplane plane = _airplaneService.GetAirplaneById(id);
            if (plane == null)
            {
                return NotFound();
            }
            return Ok(plane);
            
        }

        // POST: Airplane
        [HttpPost]
        public IActionResult Create(AirplaneDto airplane)
        {
            bool created = _airplaneService.AddAirplane(airplane);
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
        public IActionResult Delete(int id)
        {
            bool deleted = _airplaneService.DeleteAirplane(id);
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
