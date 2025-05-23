using System.Text.Json.Serialization;

namespace WifiAPIExam.Controllers.Models;

public class GetShipIdsResponseModel
{
    [JsonPropertyName("shipId")]
    public int ShipId { get; set; }
}
