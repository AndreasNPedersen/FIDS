
using System.Text.Json;
using FIDS.Backend.Domain;
using Microsoft.AspNetCore.SignalR.Client;

var apiUrl = "https://localhost:7120";
var hubUrl = apiUrl + "/hub";

// Opret en HTTP klient til at kalde API'en
using var httpClient = new HttpClient();

// Hent den nuværende liste af fly fra API'en
var response = await httpClient.GetAsync($"{apiUrl}/Departures");
response.EnsureSuccessStatusCode();
var responseBody = await response.Content.ReadAsStringAsync();
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
};
var flights = JsonSerializer.Deserialize<List<Flight>>(responseBody, options);

Console.WriteLine("Initial Flight Status:");
foreach (var flight in flights)
{
    Console.WriteLine($"Flight ID: {flight.Id}, Destination: {flight.Destination}, Status: {flight.Status}");
}
Console.WriteLine("Updated Flight Status:");
// Opret en forbindelse til SignalR hubben
var hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
hubConnection.On<List<Flight>>("ReceiveDepartureStatus", updatedFlights =>
{

    foreach (var flight in updatedFlights)
    {
        Console.WriteLine($"Flight ID: {flight.Id}, Destination: {flight.Destination}, Status: {flight.Status}");
    }
});

await hubConnection.StartAsync();
Console.WriteLine("Connected to hub. Fetching flight status...");
Console.WriteLine("Press a key to exit...");
Console.ReadKey();

await hubConnection.StopAsync();

