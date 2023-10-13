using Fly.Models.DTO;
using Fly.Models.Entities;
using Fly.Persistence;

namespace Fly.Services
{

    public interface IAirplaneService
    {
        Task<List<Airplane>> GetAllAirplaneAsync();
        Task<Airplane?> GetAirplaneByIdAsync(int id);
        Task<bool> AddAirplaneAsync(AirplaneDto airplane);
        Task<bool> DeleteAirplaneAsync(int id);
    }

    public class AirplaneService : IAirplaneService
    {
        private readonly AirplaneDbContext _db;
        private readonly ILogger<AirplaneService> _logger;
        public AirplaneService(AirplaneDbContext context, ILogger<AirplaneService> log)
        {
            _db = context;
            _logger = log;
        }

        public Task<bool> AddAirplaneAsync(AirplaneDto airplane)
        {
            return Task.Run(() =>
            {

                if (airplane == null) throw new ArgumentNullException();

                try
                {
                    Airplane airplaneEntity = new Airplane
                    {
                        Owner = airplane.Owner,
                        Type = airplane.Type,
                        MaxSeats = airplane.MaxSeats,
                        MaxVolumeCargo = airplane.MaxVolumeCargo,
                        MaxWeightCargo = airplane.MaxWeightCargo
                    };

                    _db.Airplanes.Add(airplaneEntity);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    //Logging
                    _logger.LogError("Error in " + nameof(AirplaneService) + ", creating an airplane");
                    return false;
                }
            });
        }


        public async Task<bool> DeleteAirplaneAsync(int id)
        {
            if (id < 0) throw new ArgumentNullException();
            try
            {
                Airplane airplane = await GetAirplaneByIdAsync(id);

                if (airplane == null) throw new KeyNotFoundException();

                _db.Airplanes.Remove(airplane);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in " + nameof(AirplaneService) + ", Deleting an airplane");
                return false;
            }

        }

        public async Task<Airplane> GetAirplaneByIdAsync(int id)
        {
            try
            {
                if (id < 0) throw new ArgumentNullException();
                return await _db.Airplanes.FindAsync(id);
            }
            catch (Exception ex)
            {
                //logging thing
                _logger.LogError("Error in " + nameof(AirplaneService) + ", Getting an airplane with Id:" + id);
                return null;
            }
        }

        public Task<List<Airplane>> GetAllAirplaneAsync()
        {
            return Task.Run(() =>
            {
                return _db.Airplanes.ToList();
            }
            );
        }
    }
}
