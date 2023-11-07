using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Hubs
{
    public class ArrivalsStatusHub : Hub
    {
        public async Task SendFlightStatus(List<TravelResponseDTO> travels)
        {
            await Clients.All.SendAsync("ReceiveArrivalsStatus", travels);
        }
    }
}

