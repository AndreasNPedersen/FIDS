using FlyRejser.DTO;
using FlyRejser.Helpers;

namespace FlyRejser.Service
{
    public interface IFlightServiceAPIClient
    {
        public Task<Flight> GetFlightIdAsync(int id);
    }
    public class FlightServiceAPIClient : IFlightServiceAPIClient
    {
        private FlightHelper _flightHelper;
        public FlightServiceAPIClient(ILogger logger)
        {
            _flightHelper = new FlightHelper(logger);
        }

        public async Task<Flight> GetFlightIdAsync(int id)
        {
            return await _flightHelper.GetAsync(id.ToString());
        }
        
    }
}
