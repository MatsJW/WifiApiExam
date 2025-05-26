using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database.Entities;
using WifiAPIExam.Models;

namespace WifiAPIExam.Database;

public class WifiDbContext : DbContext
{
    public WifiDbContext(DbContextOptions<WifiDbContext> options) : base(options) { }

    public DbSet<WifiDataEntity> WifiDatabase { get; set; }
    public DbSet<WifiShipEntity> ShipIds { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WifiDataEntity>()
            .HasIndex(w => new { w.ShipId, w.SellTime });

        modelBuilder.Entity<WifiShipEntity>()
            .HasKey(s => s.ShipId);
    }
}

