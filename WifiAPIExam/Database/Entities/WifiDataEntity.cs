using System.ComponentModel.DataAnnotations;

namespace WifiAPIExam.Database.Entities;

public class WifiDataEntity
{
    [Key]
    public int VoucherId { get; set; }
    public int ShipId { get; set; }
    public DateTime? SellTime { get; set; }
    public DateTime? ActivationTime { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Billing { get; set; }
    public string Category { get; set; }
    public int DataSentKB { get; set; }
    public int DataReceivedKB { get; set; }
    public string CreditCardCountry { get; set; }
    public int Devices { get; set; }
    public int VolumeQuotaMB { get; set; }
    public int TimeQuotaMinutes { get; set; }
    public decimal Fees { get; set; }
    public decimal PriceNok { get; set; }
    public int RefundNok { get; set; }
    public DateTime? RefundTime { get; set; }
    public string VatCountry { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public string DepartureCc { get; set; }
    public string DestinationCc { get; set; } 
    public string ActivationCc { get; set; }
    public int UniqueToday { get; set; }
    public int UniqueThisMonth { get; set; }
    public bool Completed { get; set; }
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

