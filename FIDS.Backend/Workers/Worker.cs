using System;
using System.Net.NetworkInformation;
using FIDS.Backend.Domain;
using FIDS.Backend.Hubs;
using FIDS.Backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace FIDS.Backend.Workers;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBoardingService _boardingService;
    private readonly IBaggageService _baggageService;
    private readonly IFlightService _flightService;
    private readonly IHubContext<DepartureStatusHub> _departureStatusHubContext;
    private List<Flight> flights = new List<Flight>();//for test only
    private static Random random = new Random();//for test only
    public Worker(ILogger<Worker> logger
        , IBoardingService boardingService
        , IBaggageService baggageService
        , IFlightService FlightService
        , IHubContext<DepartureStatusHub> departureStatusHubContext
        )
    {
        _logger = logger;
        _boardingService = boardingService;
        _baggageService = baggageService;
        _flightService = FlightService;
        _departureStatusHubContext = departureStatusHubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string flightdestination = RandomString(10);
            flights.Add(new Flight { Id = 1, Destination = flightdestination, Status = "On Time" });


            //Fetch the next 20 departures from the Travel api
            //Save to database TODO:
            //Get deparurer from database
            //Update Clients
            await _departureStatusHubContext.Clients.All.SendAsync("ReceiveDepartureStatus", flights);

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}