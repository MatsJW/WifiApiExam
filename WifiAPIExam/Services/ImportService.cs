using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database.Entities;
using WifiAPIExam.Models;

namespace WifiAPIExam.Services;

public class ImportService : IImportService
{
    private readonly IWifiRepository _repository;
    private readonly ILogger<ImportService> _logger;

    public ImportService(IWifiRepository repository, ILogger<ImportService> logger)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task ImportFromDirectoryAsync(string directoryPath)
    {
        var allShipIds = new HashSet<int>();
        // Get all JSON files in the specified directory
        var jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (var filePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            _logger.LogInformation($"Processing file: {fileName}");
            
            // Read the entire JSON file
            var fileContent = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON array into a List<WifiDataModelDto> using custom options
            List<WifiDataModelDto>? wifiDataDtos;
            try
            {
                wifiDataDtos = JsonSerializer.Deserialize<List<WifiDataModelDto>>(fileContent);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning($"Failed to deserialize file: {fileName}. Error: {ex.Message}");
                continue; // Skip if deserialization failed
            }

            if (wifiDataDtos is null)
            {
                _logger.LogWarning($"Failed to deserialize file: {fileName}. File may be empty or invalid.");
                continue; // Skip if deserialization failed or file was empty
            }

            // Map DTOs to Models
            var wifiData = wifiDataDtos
                .Select(dto => dto.MapToModel())
                .ToList();

            if (wifiData == null || wifiData.Count == 0)
            {
                _logger.LogWarning($"No valid data found in file: {fileName}. Skipping import.");
                continue; // Skip if no valid data was found
            }
            
            allShipIds.UnionWith(wifiData.Select(w => w.ShipId));
            // Add all new records
            await _repository.AddWifiDataRangeAsync(wifiData);
        }

        if (allShipIds.Count > 0)
        {
            // Get existing ship IDs from the repository
            var existingIds = await _repository.GetExistingShipIdsAsync(allShipIds);
            
            // Find new ship IDs that don't exist yet
            var newIds = allShipIds.Except(existingIds).ToList();
            
            if (newIds.Any())
            {
                // Add new ship IDs
                await _repository.AddShipIdsAsync(newIds);
            }
        }

        // Save changes to the database
        await _repository.SaveChangesAsync();
    }
}

