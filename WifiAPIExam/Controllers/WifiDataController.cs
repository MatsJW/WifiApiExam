using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database;
using WifiAPIExam.Models;

namespace WifiAPIExam.Controllers;

[ApiController]
[Route("[controller]")]
public class WifiDataController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WifiDbContext _context;

    public WifiDataController(ILogger<WeatherForecastController> logger, WifiDbContext context)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet(Name = "GetWifiData")]
    public async Task<ActionResult<IEnumerable<GetSumPerHourResponseModel>>> GetSumPerHour(
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int shipId)
    {
        if (!DateTime.TryParse(startDate, out var sd))
            return BadRequest("Invalid start date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");
        if (!DateTime.TryParse(endDate, out var ed))
            return BadRequest("Invalid end date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");

        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc);

        var hourlyData = await _context.WifiDatabase
            .Where(w => w.ShipId == shipId
                        && w.SellTime >= start
                        && w.SellTime <= end)
            .GroupBy(w => new {
                w.SellTime.Year,
                w.SellTime.Month,
                w.SellTime.Day,
                w.SellTime.Hour
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                Sum = (int)g.Sum(x => x.Price)
            })
            .ToListAsync();

        var result = hourlyData.Select(item => {
            var dtStart = new DateTime(item.Year, item.Month, item.Day, item.Hour, 0, 0, DateTimeKind.Utc);
            var dtEnd   = dtStart.AddHours(1).AddTicks(-1);
            return new GetSumPerHourResponseModel {
                StartDate  = dtStart.ToString("o"),
                EndDate    = dtEnd  .ToString("o"),
                ShipId     = shipId,
                SumPerHour = item.Sum
            };
        });

        return Ok(result);
    }
}