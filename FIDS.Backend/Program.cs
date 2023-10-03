using FIDS.Backend.Hubs;
using FIDS.Backend.Services;
using FIDS.Backend.Workers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<IBoardingService, BoardingService>();
builder.Services.AddSingleton<IBaggageService, BaggageService>();
builder.Services.AddSingleton<IFlightService, FlightService>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<DepartureStatusHub>("/hub");
    endpoints.MapControllers();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run("http://*:5000");
