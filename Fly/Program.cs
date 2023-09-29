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
                opt.UseSqlServer("Data Source=" + "flydb" + ",1433;Initial Catalog=Airplanes;User ID=SA;Password=Yourstrongpassw0rd;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

            builder.Services.AddScoped<IAirplaneService,AirplaneService>();
            builder.Services.AddCors(x => x.AddPolicy("allowall", x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            var app = builder.Build();

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