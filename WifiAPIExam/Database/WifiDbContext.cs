using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database.Entities;
using WifiAPIExam.Models;

namespace WifiAPIExam.Database;

public class WifiDbContext : DbContext
{
    public WifiDbContext(DbContextOptions<WifiDbContext> options) : base(options) { }

    public virtual DbSet<WifiDataEntity> WifiDatabase { get; set; }
    public virtual DbSet<WifiShipEntity> ShipIds { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WifiDataEntity>()
            .HasIndex(w => new { w.ShipId, w.SellTime });

        modelBuilder.Entity<WifiShipEntity>()
            .HasKey(s => s.ShipId);
    }
}

