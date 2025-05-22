namespace WifiAPIExam.Models;

public class WifiDataModelDto
{
    public string VoucherId { get; set; }
    public string ShipId { get; set; }
    public string SellTime { get; set; }
    public string ActivationTime { get; set; }
    public string Price { get; set; }
    public string Currency { get; set; }
    public string Billing { get; set; }
    public string Category { get; set; }
    public string DataSentKB { get; set; }
    public string DataReceivedKB { get; set; }
    public string CreditCardCountry { get; set; }
    public string Devices { get; set; }
    public string VolumeQuotaMB { get; set; }
    public string TimeQuotaMinutes { get; set; }
    public int Fees { get; set; }
    public string PriceNok { get; set; }
    public int RefundNok { get; set; }
    public string RefundTime { get; set; }
    public string VatCountry { get; set; }
    public string UpdatedTime { get; set; }
    public string DepartureCc { get; set; }
    public string DestinationCc { get; set; }
    public string ActivationCc { get; set; }
    public int UniqueToday { get; set; }
    public int UniqueThisMonth { get; set; }
    public string Completed { get; set; }
}

