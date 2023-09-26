using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlyRejser.Data;
namespace FlyRejser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<FlyRejserContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FlyRejserContext") ?? throw new InvalidOperationException("Connection string 'FlyRejserContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            app.UseSwagger();
            app.UseSwaggerUI(ui => ui.SwaggerEndpoint("/swagger/v1/swagger.json", "FligtTravels WEB API V1"));
            

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}