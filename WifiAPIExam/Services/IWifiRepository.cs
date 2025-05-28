using WifiAPIExam.Database.Entities;

namespace WifiAPIExam.Services;

public interface IWifiRepository
{
    /// <summary>
    /// Gets existing ship IDs that match the provided set of ship IDs
    /// </summary>
    /// <param name="shipIds">The set of ship IDs to check against the database</param>
    /// <returns>A list of ship IDs that already exist in the database</returns>
    Task<List<int>> GetExistingShipIdsAsync(HashSet<int> shipIds);
    
    /// <summary>
    /// Adds a range of WiFi data entities to the database
    /// </summary>
    /// <param name="entities">The WiFi data entities to add</param>
    Task AddWifiDataRangeAsync(IEnumerable<WifiDataEntity> entities);
    
    /// <summary>
    /// Adds new ship IDs to the database
    /// </summary>
    /// <param name="shipIds">The ship IDs to add</param>
    Task AddShipIdsAsync(IEnumerable<int> shipIds);
    
    /// <summary>
    /// Saves all pending changes to the database
    /// </summary>
    Task SaveChangesAsync();
}
