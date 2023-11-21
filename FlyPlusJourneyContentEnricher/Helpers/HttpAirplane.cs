using FlyPlusJourneyContentEnricher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPlusJourneyContentEnricher.Helpers
{
    public static class HttpAirplane
    {
        public static Airplane GetAirplane(string airplaneURL, Guid id)
        {
            try
            {
                var response = new HttpClient().GetAsync(airplaneURL + id.ToString()).Result;
                var responseStringify = response.Content.ReadAsStringAsync().Result;
                Airplane plane = JsonConvert.DeserializeObject<Airplane>(responseStringify);
                return plane;
            } catch (Exception)
            {
                return default;
            }
        }
    }
}
