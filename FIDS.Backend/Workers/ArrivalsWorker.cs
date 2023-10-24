using System;
using System.Net.NetworkInformation;
using FIDS.Backend.Domain;
using FIDS.Backend.Hubs;
using FIDS.Backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Workers;

public sealed class ArrivalsWorker : BackgroundService
{
    private readonly ILogger<ArrivalsWorker> _logger;
    private readonly IBoardingService _boardingService;
    private readonly IBaggageService _baggageService;
    private readonly IFlightService _flightService;
    private readonly IHubContext<ArrivalsStatusHub> _arrivalsStatusHubContext;
    private List<TravelResponseDTO> travels = new List<TravelResponseDTO>();//for test only
    private static Random random = new Random();//for test only
    public ArrivalsWorker(ILogger<ArrivalsWorker> logger
        , IBoardingService boardingService
        , IBaggageService baggageService
        , IFlightService FlightService
        , IHubContext<ArrivalsStatusHub> arrivalsStatusHubContext
        )
    {
        _logger = logger;
        _boardingService = boardingService;
        _baggageService = baggageService;
        _flightService = FlightService;
        _arrivalsStatusHubContext = arrivalsStatusHubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string toLocation = RandomString(10);
            string fromLocation = RandomString(10);
            int id = 1;
            travels.Add(new TravelResponseDTO(id++, toLocation, fromLocation, DateTime.Now.AddHours(2), DateTime.Now, 19));

            //Fetch the next 20 departures from the Travel api
            //Save to database TODO:
            //Get departure from database
            //Update Clients
            _logger.LogInformation("Notify clients about departure updates");
            await _arrivalsStatusHubContext.Clients.All.SendAsync("ReceiveArrivalsStatus", travels);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}