using Fly.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fly.Persistence
{
    public class AirplaneDbContext : DbContext
    {
        public DbSet<Airplane> Airplanes { get; set; }

        public AirplaneDbContext(DbContextOptions<AirplaneDbContext> options) : base(options)
        {

        }
    }
}
