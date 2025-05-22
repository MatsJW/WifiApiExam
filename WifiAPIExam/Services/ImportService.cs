using System.Text.Json;
using WifiAPIExam.Mappers;
using WifiAPIExam.Models;

namespace WifiAPIExam.Services;

public class ImportService :IImportService
{
    private readonly Database.WifiDbContext _context;

    public ImportService(Database.WifiDbContext context)
    {
        _context = context;
    }
    
    public async Task ImportFromDirectoryAsync(string directoryPath)
    {
        // Get all JSON files in the specified directory
        var jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (var filePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            Console.WriteLine($"Processing file: {fileName}");
            
            // Read the entire JSON file
            var fileContent = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON array into a List<Prices> using custom options
            var wifiDataDtos = JsonSerializer.Deserialize<List<WifiDataModelDto>>(fileContent);

            if (wifiDataDtos is null)
            {
                Console.WriteLine("Deserialization failed or file was empty.");
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