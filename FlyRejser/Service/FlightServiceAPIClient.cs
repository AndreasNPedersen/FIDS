using FlyRejser.DTO;
using FlyRejser.Helpers;

namespace FlyRejser.Service
{
    public interface IFlightServiceAPIClient
    {
        public Task<Flight> GetFlightIdAsync(Guid id);
    }
    public class FlightServiceAPIClient : IFlightServiceAPIClient
    {
        private FlightHelper _flightHelper;
        public FlightServiceAPIClient(ILogger logger)
        {
            _flightHelper = new FlightHelper(logger);
        }

        public async Task<Flight> GetFlightIdAsync(Guid id)
        {
            return await _flightHelper.GetAsync(id.ToString());
        }
        
    }
}
