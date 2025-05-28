using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database;
using WifiAPIExam.Database.Entities;

namespace WifiAPIExam.Services;

public class WifiRepository : IWifiRepository
{
    private readonly WifiDbContext _context;
    
    public WifiRepository(WifiDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<int>> GetExistingShipIdsAsync(HashSet<int> shipIds)
    {
        return await _context.ShipIds
            .Where(s => shipIds.Contains(s.ShipId))
            .Select(s => s.ShipId)
            .ToListAsync();
    }
    
    public Task AddWifiDataRangeAsync(IEnumerable<WifiDataEntity> entities)
    {
        _context.WifiDatabase.AddRange(entities);
        return Task.CompletedTask;
    }
    
    public Task AddShipIdsAsync(IEnumerable<int> shipIds)
    {
        var shipIdEntities = shipIds.Select(id => new WifiShipEntity { ShipId = id });
        _context.ShipIds.AddRange(shipIdEntities);
        return Task.CompletedTask;
    }
    
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
