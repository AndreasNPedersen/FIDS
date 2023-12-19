using System.Text.Json;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR.Client;

var apiUrl = Environment.GetEnvironmentVariable("BACKEND_URL");
var hubUrl = apiUrl + "/arrivalsHub";


// Opret en HTTP klient til at kalde API'en
using var httpClient = new HttpClient();

// Hent den nuværende liste af fly fra API'en
var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
while (!cts.IsCancellationRequested)
{
    try
    {
        var response = await httpClient.GetAsync($"{apiUrl}/Arrivals", cts.Token);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var travels = JsonSerializer.Deserialize<List<TravelResponseDTO>>(responseBody, options);

        Console.WriteLine("Initial arrivals travel status:");
        foreach (var travel in travels)
        {
            Console.WriteLine($"Flight ID: {travel.Id}, From: {travel.FromLocation}, To: {travel.Id}, DepartureTime: {travel.DepartureDate}, ArrivalTime: {travel.ArrivalDate}");
        }
        break;
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("CRITICAL: No connection available...");
        continue;
    }
    catch (TaskCanceledException ex)
    {
        Console.WriteLine("CRITICAL: Timeout fetching data on startup");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}




Console.WriteLine("Updated arrival travel status:");
// Opret en forbindelse til SignalR hubben
var hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
hubConnection.On<List<TravelResponseDTO>>("ReceiveArrivalsStatus", updatedFlights =>
{

    foreach (var flight in updatedFlights)
    {
        Console.WriteLine($"Flight ID: {flight.Id}, From: {flight.FromLocation}, To: {flight.Id}, DepartureTime: {flight.DepartureDate}, ArrivalTime: {flight.ArrivalDate}");
    }
});

cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
while (!cts.IsCancellationRequested)
{
    try
    {
        await hubConnection.StartAsync(cts.Token);
        break;
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("CRITICAL: No connection available");
        continue;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex.GetType()} {ex.Message}");
        continue;
    }
}

if (cts.IsCancellationRequested)
{
    Console.WriteLine("Could not connect to hub. Exiting");
    Environment.Exit(0);
}

Console.WriteLine("Connected to hub. Fetching flight status...");
//Console.WriteLine("Press a key to exit...");
//Console.ReadKey();

//await hubConnection.StopAsync();

while (true)
{
    await Task.CompletedTask;
}