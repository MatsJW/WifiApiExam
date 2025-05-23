using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Models;

namespace WifiAPIExam.Database;

public class WifiDbContext : DbContext
{
    public WifiDbContext(DbContextOptions<WifiDbContext> options) : base(options) { }

    public DbSet<WifiDataModel> WifiDatabase { get; set; }
    public DbSet<WifiShipIdModel> ShipIds { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WifiDataModel>()
            .HasIndex(w => new { w.ShipId, w.SellTime });

        modelBuilder.Entity<WifiShipIdModel>()
            .HasKey(s => s.ShipId);
    }
}

