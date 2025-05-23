using WifiAPIExam.Models;

namespace WifiAPIExam.Mappers;

public static class WifiDataModelMapper
{
    public static WifiDataModel MapToModel(WifiDataModelDto dto)
    {
        return new WifiDataModel
        {
            VoucherId = int.Parse(dto.VoucherId),
            ShipId = int.Parse(dto.ShipId),
            SellTime = ParseDateTimeOrDefault(dto.SellTime),
            ActivationTime = ParseDateTimeOrDefault(dto.ActivationTime),
            Price = decimal.Parse(dto.Price),
            Currency = dto.Currency,
            Billing = dto.Billing,
            Category = dto.Category,
            DataSentKB = int.Parse(dto.DataSentKB),
            DataReceivedKB = int.Parse(dto.DataReceivedKB),
            CreditCardCountry = dto.CreditCardCountry,
            Devices = int.Parse(dto.Devices),
            VolumeQuotaMB = int.Parse(dto.VolumeQuotaMB),
            TimeQuotaMinutes = int.Parse(dto.TimeQuotaMinutes),
            Fees = dto.Fees,
            PriceNok = decimal.Parse(dto.PriceNok),
            RefundNok = dto.RefundNok,
            RefundTime = ParseDateTimeOrDefault(dto.RefundTime),
            VatCountry = dto.VatCountry,
            UpdatedTime = ParseDateTimeOrDefault(dto.UpdatedTime),
            DepartureCc = dto.DepartureCc,
            DestinationCc = dto.DestinationCc,
            ActivationCc = dto.ActivationCc,
            UniqueToday = dto.UniqueToday,
            UniqueThisMonth = dto.UniqueThisMonth,
            Completed = dto.Completed == "1" || dto.Completed.ToLower() == "true"
        };
    }

    private static DateTime ParseDateTimeOrDefault(string value)
    {
        if (DateTime.TryParse(value, out var dt))
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
    }
}