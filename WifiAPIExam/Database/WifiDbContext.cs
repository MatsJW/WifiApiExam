using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Models;

namespace WifiAPIExam.Database;

public class WifiDbContext : DbContext
{
    public WifiDbContext(DbContextOptions<WifiDbContext> options) : base(options) { }

    public DbSet<WifiDataModel> WifiDatabase { get; set; }
} 