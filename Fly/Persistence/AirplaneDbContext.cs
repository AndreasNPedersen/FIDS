using Fly.Models.Entities;
using Fly.Services;
using Microsoft.EntityFrameworkCore;

namespace Fly.Persistence
{
    public class AirplaneDbContext : DbContext
    {
        public virtual DbSet<Airplane> Airplanes { get; set; }

        public AirplaneDbContext(DbContextOptions<AirplaneDbContext> options) : base(options)
        {

        }
        public AirplaneDbContext()
        {

        }

    }
}
