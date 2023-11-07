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
    public static class ArrivelService
    {
        // lav flightdeps til en redis
        public static void SendEnrichedFJ(string message, IDatabase redis, WatsonWsServer _server, List<Guid> clients)
        {
            FlightFIDSArrivalDTO fIDSDTO = JsonConvert.DeserializeObject<FlightFIDSArrivalDTO>(message);
            string departures = redis.StringGet("Arrival");
            List<FlightFIDSArrivalDTO> flightDeps = !String.IsNullOrEmpty(departures) ?
                JsonConvert.DeserializeObject<List<FlightFIDSArrivalDTO>>(departures) :
                new List<FlightFIDSArrivalDTO>();
            flightDeps.RemoveAll(x => x.ArrivalDate.Day < DateTime.Now.Day - 1); //removes old flights from memory
                                                                                   
            var fJ = flightDeps.Find(x => x.FlightJourneyId == fIDSDTO.FlightJourneyId);
            if (fJ != null && fJ.FlightJourneyId == fIDSDTO.FlightJourneyId)
            {
                flightDeps[flightDeps.IndexOf(fJ)] = fIDSDTO;
            }
            else
            {
                flightDeps.Add(fIDSDTO);
            }
            redis.StringSet("Arrival", JsonConvert.SerializeObject(flightDeps));
            WebSocketFIDS.SendMessageArrivalToFrontend(flightDeps, _server, clients);
        }
    }
}
