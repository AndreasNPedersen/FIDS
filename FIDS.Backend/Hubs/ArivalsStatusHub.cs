using System;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Hubs
{
    public class ArivalsStatusHub : Hub
    {
        public async Task SendFlightStatus(List<TravelResponseDTO> travels)
        {
            await Clients.All.SendAsync("ReceiveArivalsStatus", travels);
        }
    }
}

