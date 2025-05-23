namespace WifiAPIExam.Controllers;

public class GetSumPerHourResponseModel
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public int ShipId { get; set; }
    public int SumPerHour { get; set; }
}