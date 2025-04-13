using Microsoft.EntityFrameworkCore;
using FlightSimulator.Models;

namespace FlightSimulator.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=flights.db");
        }
    }
}
