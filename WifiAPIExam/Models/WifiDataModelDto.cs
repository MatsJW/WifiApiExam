using System.Text.Json.Serialization;

namespace WifiAPIExam.Models;

public class WifiDataModelDto
{
    public required string VoucherId { get; set; }
    public required string ShipId { get; set; }
    public required string SellTime { get; set; }
    public required string ActivationTime { get; set; }
    public required string Price { get; set; }
    public required string Currency { get; set; }
    public required string Billing { get; set; }
    public required string Category { get; set; }
    
    [JsonPropertyName("DataSentKB")]
    public required string DataSentInKiloBytes { get; set; }
    
    [JsonPropertyName("DataReceivedKB")]
    public required string DataReceivedInKiloBytes { get; set; }
    public required string CreditCardCountry { get; set; }
    public required string Devices { get; set; }
    public required string VolumeQuotaMB { get; set; }
    public required string TimeQuotaMinutes { get; set; }
    public int Fees { get; set; }
    public required string PriceNok { get; set; }
    public int RefundNok { get; set; }
    public required string RefundTime { get; set; }
    public required string VatCountry { get; set; }
    public required string UpdatedTime { get; set; }
    public required string DepartureCc { get; set; }
    public required string DestinationCc { get; set; }
    public required string ActivationCc { get; set; }
    public int UniqueToday { get; set; }
    public int UniqueThisMonth { get; set; }
    public required string Completed { get; set; }
    
    
    public WifiDataModel MapToModel()
    {
        return new WifiDataModel
        {
            VoucherId = int.Parse(VoucherId),
            ShipId = int.Parse(ShipId),
            SellTime = ParseDateTimeOrDefault(SellTime),
            ActivationTime = ParseDateTimeOrDefault(ActivationTime),
            Price = decimal.Parse(Price),
            Currency = Currency,
            Billing = Billing,
            Category = Category,
            DataSentKB = int.Parse(DataSentInKiloBytes),
            DataReceivedKB = int.Parse(DataReceivedInKiloBytes),
            CreditCardCountry = CreditCardCountry,
            Devices = int.Parse(Devices),
            VolumeQuotaMB = int.Parse(VolumeQuotaMB),
            TimeQuotaMinutes = int.Parse(TimeQuotaMinutes),
            Fees = Fees,
            PriceNok = decimal.Parse(PriceNok),
            RefundNok = RefundNok,
            RefundTime = ParseDateTimeOrDefault(RefundTime),
            VatCountry = VatCountry,
            UpdatedTime = ParseDateTimeOrDefault(UpdatedTime),
            DepartureCc = DepartureCc,
            DestinationCc = DestinationCc,
            ActivationCc = ActivationCc,
            UniqueToday = UniqueToday,
            UniqueThisMonth = UniqueThisMonth,
            Completed = Completed == "1" || Completed.ToLower() == "true"
        };
    }

    private static DateTime ParseDateTimeOrDefault(string value)
    {
        if (DateTime.TryParse(value, out var dt))
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
    }
}

