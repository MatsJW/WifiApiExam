using System.Text.Json.Serialization;

namespace WifiAPIExam.Controllers.Models;

public class GetSalesResponseModel
{
    [JsonPropertyName("startDate")]
    public required string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public required string EndDate { get; set; }

    [JsonPropertyName("shipId")]
    public int ShipId { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}