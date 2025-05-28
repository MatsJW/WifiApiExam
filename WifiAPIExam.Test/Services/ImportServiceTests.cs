using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WifiAPIExam.Database.Entities;
using WifiAPIExam.Models;
using WifiAPIExam.Services;

namespace WifiAPIExam.Test.Services;

public class ImportServiceTests
{
    private readonly Mock<ILogger<ImportService>> _mockLogger;
    private readonly Mock<IWifiRepository> _mockRepository;
    private readonly ImportService _importService;

    public ImportServiceTests()
    {
        // Setup mock logger
        _mockLogger = new Mock<ILogger<ImportService>>();
        
        // Create mock repository
        _mockRepository = new Mock<IWifiRepository>();
        
        // Create the service with our mocked dependencies
        _importService = new ImportService(_mockRepository.Object, _mockLogger.Object);
    }
    
    [Fact]
    public async Task ImportFromDirectoryAsync_WithValidFiles_ImportsData()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            // Create test JSON file with sample data
            var testData = new List<WifiDataModelDto>
            {
                new WifiDataModelDto 
                { 
                    VoucherId = "8703316",
                    ShipId = "678092675",
                    SellTime = "2025-04-01 06:36:48",
                    ActivationTime = "2025-04-01 06:36:47",
                    Price = "10",
                    Currency = "EUR",
                    Billing = "Cash",
                    Category = "Pax",
                    DataSentInKiloBytes = "89467",
                    DataReceivedInKiloBytes = "617777",
                    CreditCardCountry = "",
                    Devices = "1",
                    VolumeQuotaMB = "0",
                    TimeQuotaMinutes = "720",
                    Fees = 0,
                    PriceNok = "0",
                    RefundNok = 0,
                    RefundTime = "0000-00-00 00:00:00",
                    VatCountry = "ee",
                    UpdatedTime = "2025-04-02 08:31:27",
                    DepartureCc = "ee",
                    DestinationCc = "se",
                    ActivationCc = "se",
                    UniqueToday = 0,
                    UniqueThisMonth = 0,
                    Completed = "1"
                },
                new WifiDataModelDto 
                { 
                    VoucherId = "8703317",
                    ShipId = "678092676",
                    SellTime = "2025-04-02 07:45:12",
                    ActivationTime = "2025-04-02 07:45:13",
                    Price = "15",
                    Currency = "EUR",
                    Billing = "Credit",
                    Category = "Pax",
                    DataSentInKiloBytes = "120256",
                    DataReceivedInKiloBytes = "528691",
                    CreditCardCountry = "no",
                    Devices = "2",
                    VolumeQuotaMB = "0",
                    TimeQuotaMinutes = "1440",
                    Fees = 0,
                    PriceNok = "163.50",
                    RefundNok = 0,
                    RefundTime = "0000-00-00 00:00:00",
                    VatCountry = "no",
                    UpdatedTime = "2025-04-03 09:22:14",
                    DepartureCc = "se",
                    DestinationCc = "no",
                    ActivationCc = "se",
                    UniqueToday = 0,
                    UniqueThisMonth = 0,
                    Completed = "1"
                }
            };
            
            var testFilePath = Path.Combine(tempDir, "2025-04_123456.json");
            await File.WriteAllTextAsync(testFilePath, JsonSerializer.Serialize(testData));
            
            // Set up repository mocks for a simpler approach that avoids EF Core extension method issues
            _mockRepository.Setup(x => x.GetExistingShipIdsAsync(It.IsAny<HashSet<int>>()))
                .ReturnsAsync(new List<int>());
                
            // Act
            await _importService.ImportFromDirectoryAsync(tempDir);
            
            // Assert - Verify that AddWifiDataRangeAsync was called on the repository
            _mockRepository.Verify(m => m.AddWifiDataRangeAsync(It.IsAny<IEnumerable<WifiDataEntity>>()), Times.Once);
            _mockRepository.Verify(m => m.AddShipIdsAsync(It.IsAny<IEnumerable<int>>()), Times.Once);
            _mockRepository.Verify(m => m.SaveChangesAsync(), Times.Once);
        }
        finally
        {
            // Clean up temp directory
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
    
    [Fact]
    public async Task ImportFromDirectoryAsync_WithInvalidFiles_LogsWarning()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            // Create test file with invalid JSON structure
            var testFilePath = Path.Combine(tempDir, "invalid.json");
            // This will fail because it's missing required properties
            await File.WriteAllTextAsync(testFilePath, "[{\"someField\": \"someValue\"}]");
            
            // Setup logging to capture logs
            _mockLogger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
            
            // Act
            await _importService.ImportFromDirectoryAsync(tempDir);
            
            // Assert - verify that appropriate warning was logged
            // We're now expecting a "Failed to deserialize file" warning due to missing required properties
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to deserialize file")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
        finally
        {
            // Clean up temp directory
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
