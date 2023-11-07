using FIDS.Backend.Hubs;
using FIDS.Backend.Services;
using FIDS.Backend.Workers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<IBoardingService, BoardingService>();
builder.Services.AddSingleton<IBaggageService, BaggageService>();
builder.Services.AddSingleton<IFlightService, FlightService>();
builder.Services.AddHostedService<DepartureWorker>();
builder.Services.AddHostedService<ArrivalsWorker>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
Console.WriteLine($"LogStash url: {Environment.GetEnvironmentVariable("LOGSTASH_URL")}");
builder.Host.UseSerilog((_, lc) => lc.WriteTo.Console().WriteTo.Http(Environment.GetEnvironmentVariable("LOGSTASH_URL") ?? "", null));

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<DeparturesStatusHub>("/departuresHub");
    endpoints.MapHub<ArrivalsStatusHub>("/arrivalsHub");
    endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run("http://*:5000");
