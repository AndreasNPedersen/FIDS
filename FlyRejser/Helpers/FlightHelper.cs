using FlyRejser.DTO;
using Newtonsoft.Json;

namespace FlyRejser.Helpers
{
    public class FlightHelper
    {
        private HttpClient _httpClient;
        private string _uri;
        private ILogger _log;
        public FlightHelper(ILogger logger) { 
            _httpClient = new HttpClient();
            _uri = Environment.GetEnvironmentVariable("FlightURI");
            _log = logger;
        }

        public async Task<Flight> GetAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://"+_uri+"/airplane/"+ query);
                var responseBodyJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(responseBodyJson);

            } catch (Exception ex)
            {
                _log.LogError("The FlightHelper request was: " + ex.Message);
                return default;
            }
        }

        public Task PostAsync(string jsonObject)
        {
            return Task.Run(() => _httpClient.PostAsJsonAsync(_uri,jsonObject));
        }

        public Task Delete(string query)
        {
            return Task.Run(() => _httpClient.GetAsync(_uri + "/" + query));
        }
    }
}
