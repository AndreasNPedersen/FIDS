using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlyRejser.Data;
using FlyRejser.Worker;

namespace FlyRejser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContextPool<FlyRejserContext>(opt =>
               opt.UseSqlServer($"Data Source={Environment.GetEnvironmentVariable("DatabaseIp")},1433;Initial Catalog=FlightJourneys;User ID=sa;Password=yourStrong(!)Password;Connect Timeout=30;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            
            builder.Services.AddCors(x => x.AddPolicy("allowall",
                x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
            // Add services to the container.
           // builder.Services.AddHostedService<WorkerGateUpdates>();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var airplaneContext = scope.ServiceProvider.GetRequiredService<FlyRejserContext>();
                airplaneContext.Database.EnsureCreated();
            }
            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("allowall");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}