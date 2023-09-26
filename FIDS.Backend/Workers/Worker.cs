using FIDS.Backend.Services;

namespace FIDS.Backend.Workers;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBoardingService _boardingService;
    private readonly IBaggageService _baggageService;
    private readonly IFlightService _flightService;

    public Worker(ILogger<Worker> logger
        , IBoardingService boardingService
        , IBaggageService baggageService
        , IFlightService FlightService
        )
    {
        _logger = logger;
        _boardingService = boardingService;
        _baggageService = baggageService;
        _flightService = FlightService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Fetch the next 20 departures from the Travel api
            // 
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}