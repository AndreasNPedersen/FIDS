using Fly.Models.DTO;
using Fly.Models.Entities;
using Fly.Persistence;

namespace Fly.Services
{

    public interface IAirplaneService
    {
        List<Airplane> GetAllAirplane();
        Airplane GetAirplaneById(int id);
        bool AddAirplane(AirplaneDto airplane);
        bool DeleteAirplane(int id);  
    }

    public class AirplaneService : IAirplaneService
    {
        private readonly AirplaneDbContext _db;
        private readonly ILogger<AirplaneService> _logger;
        public AirplaneService(AirplaneDbContext context, ILogger<AirplaneService> log) { 
            _db = context;
            _logger = log;
        }

        public bool AddAirplane(AirplaneDto airplane)
        {
            if (airplane == null) throw new ArgumentNullException();

            try
            {
                Airplane airplaneEntity = new Airplane { 
                    Owner = airplane.Owner,
                    Type = airplane.Type,
                    MaxSeats = airplane.MaxSeats,
                    MaxVolumeCargo = airplane.MaxVolumeCargo,
                    MaxWeightCargo = airplane.MaxWeightCargo
                };

                _db.Airplanes.Add(airplaneEntity);
                _db.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                //Logging
                _logger.LogError("Error in " + nameof(AirplaneService) + ", creating an airplane");
                return false;
            }
        }

        public bool DeleteAirplane(int id)
        {
            if (id < 0) throw new ArgumentNullException();
            try
            {
                _db.Airplanes.Remove(GetAirplaneById(id));
                _db.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                _logger.LogError("Error in " + nameof(AirplaneService) + ", Deleting an airplane");
                return false;
            }
        }

        public Airplane GetAirplaneById(int id)
        {
            try
            {
                if (id < 0) throw new ArgumentNullException();
                return _db.Airplanes.Where(plane => plane.Id == id).Single();
            } catch (Exception ex) {
                //logging thing
                _logger.LogError("Error in " + nameof(AirplaneService) + ", Getting an airplane with Id:" + id);
                return default;
            }
        }

        public List<Airplane> GetAllAirplane()
        {
            return _db.Airplanes.ToList();
        }
    }
}
