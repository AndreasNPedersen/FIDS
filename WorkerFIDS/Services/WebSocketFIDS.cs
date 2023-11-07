using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;
using WorkerFIDS.Models;

namespace WorkerFIDS.Services
{
    public static class WebSocketFIDS
    {
        public static void SendMessageToFrontendDeparture(List<FlightFIDSDepartDTO> flights, WatsonWsServer server, List<Guid> clients)
        {
            var currentFligts = flights.Where(x => x.DepartureTime.Day == DateTime.Now.Day).ToList();
            foreach (var client in clients)
            {
                server.SendAsync(client, JsonConvert.SerializeObject(currentFligts.OrderByDescending(x => x.DepartureTime)));
            }
        }

        public static void SendMessageArrivalToFrontend(List<FlightFIDSArrivalDTO> flights, WatsonWsServer server, List<Guid> clients)
        {
            var currentFligts = flights.Where(x => x.ArrivalDate.Day == DateTime.Now.Day).ToList();
            foreach (var client in clients)
            {
                server.SendAsync(client, JsonConvert.SerializeObject(currentFligts.OrderByDescending(x => x.ArrivalDate)));
            }
        }
    }
}
