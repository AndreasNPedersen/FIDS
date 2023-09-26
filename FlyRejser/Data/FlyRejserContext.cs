using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FlyRejser.Data
{
    public class FlyRejserContext : DbContext
    {
        public FlyRejserContext (DbContextOptions<FlyRejserContext> options)
            : base(options)
        {
        }

        public DbSet<Travel> Travel { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Travel>().ToTable(nameof(Travel), "FLIGHT");
        }
    }
}
