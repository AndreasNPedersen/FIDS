using System;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Hubs
{
    public class DepartureStatusHub : Hub
    {
        public async Task SendFlightStatus(List<Flight> flights)
        {
            await Clients.All.SendAsync("ReceiveDepartureStatus", flights);
        }
    }
}

