using FlyRejser.DTO;
using FlyRejser.Helpers;

namespace FlyRejser.Service
{
    internal class FlightServiceAPIClient
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
