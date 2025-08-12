using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Models;

namespace EventManagementAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
    }
}