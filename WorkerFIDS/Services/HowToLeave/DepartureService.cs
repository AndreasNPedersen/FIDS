using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;
using WorkerFIDS.Models;

namespace WorkerFIDS.Services.HowToLeave
{
    public static class DepartureService
    {
        // lav flightdeps til en redis
        public static void SendEnrichedFJ(string message, IDatabase redis, WatsonWsServer server, List<Guid> clients)
        {
            FlightFIDSDepartDTO fIDSDTO = JsonConvert.DeserializeObject<FlightFIDSDepartDTO>(message);
            string departures = redis.StringGet("Departs");
            List<FlightFIDSDepartDTO> flightDeps = !String.IsNullOrEmpty(departures) ? 
                JsonConvert.DeserializeObject<List<FlightFIDSDepartDTO>>(departures) : 
                new List<FlightFIDSDepartDTO>();

            flightDeps.RemoveAll(x => x.DepartureTime.Day < DateTime.Now.Day - 1); //removes old flights from memory
                                                                                    
            var fJ = flightDeps.Find(x => x.FlightJourneyId == fIDSDTO.FlightJourneyId);
            if (fJ != null && fJ.FlightJourneyId == fIDSDTO.FlightJourneyId)
            {
                flightDeps[flightDeps.IndexOf(fJ)] = fIDSDTO;
            }
            else
            {
                flightDeps.Add(fIDSDTO);
            }
            redis.StringSet("Departs", JsonConvert.SerializeObject(flightDeps));
            WebSocketFIDS.SendMessageToFrontendDeparture(flightDeps, server, clients);
        }

        
    }
}
