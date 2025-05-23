namespace WifiAPIExam.Controllers.Models;

public class GetCountPerHourResponseModel
{
    public string StartDate   { get; set; }
    public string EndDate     { get; set; }
    public int    ShipId      { get; set; }
    public int    CountPerHour { get; set; }
}