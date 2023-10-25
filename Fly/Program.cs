using Fly.Persistence;
using Fly.Services;
using Microsoft.EntityFrameworkCore;

namespace Fly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContextPool<AirplaneDbContext>(opt =>
                opt.UseSqlServer($"Data Source={Environment.GetEnvironmentVariable("DatabaseIp")},1433;Initial Catalog=Airplanes;User ID=sa;Password=yourStrong(!)Password;Connect Timeout=30;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            
            builder.Services.AddScoped<IAirplaneService,AirplaneService>();
            builder.Services.AddCors(x => x.AddPolicy("allowall",
                x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var airplaneContext = scope.ServiceProvider.GetRequiredService<AirplaneDbContext>();
                    airplaneContext.Database.EnsureCreated();
                    airplaneContext.Seed();
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