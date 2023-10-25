//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WatsonWebsocket;
//using WorkerFIDS.Models;

//namespace WorkerFIDS.Services
//{
//    public static class WebSocketFIDS
//    {
//        public static void SendMessageToFrontend(List<FlightFIDSDepartDTO> flights, WatsonWsServer server)
//        {
//            var currentFligts = flights.Where(x=>x.DepartureTime.Day == DateTime.Now.Day).ToList();
//            foreach (var client in server.ListClients())
//            {
//               server.SendAsync(client.Guid, JsonConvert.SerializeObject(currentFligts.OrderByDescending(x=>x.DepartureTime)));
//            }
//        }
//    }
//}
