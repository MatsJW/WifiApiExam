using System.Text.Json.Serialization;

namespace WifiAPIExam.Controllers.Models;

public class GetDataUsageResponseModel
{
    [JsonPropertyName("startDate")]
    public required string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public required string EndDate { get; set; }
    
    [JsonPropertyName("shipId")]
    public int ShipId { get; set; }
    
    [JsonPropertyName("DataSentKb")]
    public int DataSent { get; set; }
    
    [JsonPropertyName("DataReceivedKb")]
    public int DataReceived { get; set; }
    
    [JsonPropertyName("DataUsageKb")]
    public int DataUsage { get; set; }
}