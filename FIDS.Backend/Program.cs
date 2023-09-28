
using FIDS.Backend.Hubs;
using FIDS.Backend.Services;
using FIDS.Backend.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddTransient<IBoardingService, BoardingService>();
builder.Services.AddTransient<IBaggageService, BaggageService>();
builder.Services.AddTransient<IFlightService, FlightService>();
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
app.Run();