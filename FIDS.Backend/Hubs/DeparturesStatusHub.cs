using System;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Hubs
{
    public class DeparturesStatusHub : Hub
    {
        public async Task SendFlightStatus(List<TravelResponseDTO> travels)
        {
            await Clients.All.SendAsync("ReceiveDepartureStatus", travels);
        }
    }
}

