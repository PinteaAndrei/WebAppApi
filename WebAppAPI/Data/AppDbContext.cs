using Microsoft.EntityFrameworkCore;
using WebAppAPI.Models;

namespace WebAppAPI.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Users> Users { get; set; }
    public DbSet<Restaurants> Restaurants { get; set; }
    public DbSet<Reviews> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
