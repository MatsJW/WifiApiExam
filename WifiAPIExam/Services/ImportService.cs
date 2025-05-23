using System.Text.Json;
using WifiAPIExam.Mappers;
using WifiAPIExam.Models;

namespace WifiAPIExam.Services;

public class ImportService :IImportService
{
    private readonly Database.WifiDbContext _context;
    private readonly ILogger<ImportService> _logger;

    public ImportService(Database.WifiDbContext context , ILogger<ImportService> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task ImportFromDirectoryAsync(string directoryPath)
    {
        // Get all JSON files in the specified directory
        var jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (var filePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            _logger.LogInformation($"Processing file: {fileName}");
            
            // Read the entire JSON file
            var fileContent = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON array into a List<Prices> using custom options
            var wifiDataDtos = JsonSerializer.Deserialize<List<WifiDataModelDto>>(fileContent);

            if (wifiDataDtos is null)
            {
                _logger.LogWarning($"Failed to deserialize file: {fileName}. File may be empty or invalid.");
                continue; // Skip if deserialization failed or file was empty
            }

            // Map DTOs to Models
            var wifiData = wifiDataDtos
                .Select(WifiDataModelMapper.MapToModel)
                .ToList();
            
            // Add all new records to EF Core tracking
            _context.WifiDatabase.AddRange(wifiData);
        }

        // Save changes to the database
        await _context.SaveChangesAsync();
    }
}