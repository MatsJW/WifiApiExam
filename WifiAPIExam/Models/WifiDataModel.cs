using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WifiAPIExam.Models;

public class WifiDataModel
{
    [Key]
    public required int VoucherId { get; set; }
    public required int ShipId { get; set; }
    public required DateTime SellTime { get; set; }
    public required DateTime ActivationTime { get; set; }
    public required decimal Price { get; set; }
    public required string Currency { get; set; }
    public required string Billing { get; set; }
    public required string Category { get; set; }
    public required int DataSentKB { get; set; }
    public required int DataReceivedKB { get; set; }
    public required string CreditCardCountry { get; set; }
    public required int Devices { get; set; }
    public required int VolumeQuotaMB { get; set; }
    public required int TimeQuotaMinutes { get; set; }
    public required decimal Fees { get; set; }
    public required decimal PriceNok { get; set; }
    public required int RefundNok { get; set; }
    public required DateTime RefundTime { get; set; }
    public required string VatCountry { get; set; }
    public required DateTime UpdatedTime { get; set; }
    public required string DepartureCc { get; set; }
    public required string DestinationCc { get; set; } 
    public required string ActivationCc { get; set; }
    public required int UniqueToday { get; set; }
    public required int UniqueThisMonth { get; set; }
    public required bool Completed { get; set; }
}

// {
// "VoucherId": "8703280",
// "ShipId": "123896564",
// "SellTime": "2025-04-01 06:07:27",
// "ActivationTime": "2025-04-01 06:07:34",
// "Price": "9",
// "Currency": "EUR",
// "Billing": "CreditCard",
// "Category": "Pax",
// "DataSentKB": "189517",
// "DataReceivedKB": "2365057",
// "CreditCardCountry": "LT",
// "Devices": "1",
// "VolumeQuotaMB": "0",
// "TimeQuotaMinutes": "1320",
// "Fees": 0,
// "PriceNok": "0",
// "RefundNok": 0,
// "RefundTime": "0000-00-00 00:00:00",
// "VatCountry": "lt",
// "UpdatedTime": "2025-04-03 06:31:08",
// "DepartureCc": "lt",
// "DestinationCc": "de",
// "ActivationCc": "zz",
// "UniqueToday": 0,
// "UniqueThisMonth": 0,
// "Completed": "1"
// },