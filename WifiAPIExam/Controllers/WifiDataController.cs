using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Controllers.Models;
using WifiAPIExam.Database;
using WifiAPIExam.Models;

namespace WifiAPIExam.Controllers;

[ApiController]
[Route("Wifi")]
public class WifiDataController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WifiDbContext _context;

    public WifiDataController(ILogger<WeatherForecastController> logger, WifiDbContext context)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet("HourlyReport/Sum")]
    public async Task<ActionResult<IEnumerable<GetSumPerHourResponseModel>>> GetSumPerHour(
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int shipId)
    {
        if (!DateTime.TryParse(startDate, out var sd))
            return BadRequest("Invalid start date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");
        if (!DateTime.TryParse(endDate, out var ed))
            return BadRequest("Invalid end date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");

        // after parsing and specifying kind
        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);

        var hourlyData = await _context.WifiDatabase
            .Where(w => w.ShipId == shipId
                        && w.SellTime >= start
                        && w.SellTime <  end)
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
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ThenBy(x => x.Hour)
            .ToListAsync();

        // build all hours in the range
        var totalHours = (int)(end - start).TotalHours;
        var allHours = Enumerable.Range(0, totalHours)
            .Select(i => start.AddHours(i))
            .ToList();
        
        var result = allHours.Select(dt =>
        {
            var match = hourlyData.FirstOrDefault(h =>
                h.Year == dt.Year &&
                h.Month == dt.Month &&
                h.Day == dt.Day &&
                h.Hour == dt.Hour);

            var sum = match?.Sum ?? 0;
            var dtStart = dt;
            var dtEnd   = dtStart.AddHours(1).AddTicks(-1);

            return new GetSumPerHourResponseModel
            {
                StartDate  = dtStart.ToString("o"),
                EndDate    = dtEnd  .ToString("o"),
                ShipId     = shipId,
                SumPerHour = sum
            };
        });

        return Ok(result);
    }
    
    [HttpGet("HourlyReport/Count")]
    public async Task<ActionResult<IEnumerable<GetCountPerHourResponseModel>>> GetCountPerHour(
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int shipId)
    {
        if (!DateTime.TryParse(startDate, out var sd))
            return BadRequest("Invalid start date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");
        if (!DateTime.TryParse(endDate, out var ed))
            return BadRequest("Invalid end date format. Use 'yyyy-MM-dd' or 'yyyy-MM-ddTHH:mm:ssZ' format.");

        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);

        var hourlyData = await _context.WifiDatabase
            .Where(w => w.ShipId == shipId
                        && w.SellTime >= start
                        && w.SellTime <  end)
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
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ThenBy(x => x.Hour)
            .ToListAsync();

        var totalHours = (int)(end - start).TotalHours;
        var allHours = Enumerable.Range(0, totalHours)
            .Select(i => start.AddHours(i))
            .ToList();

        var result = allHours.Select(dt =>
        {
            var match = hourlyData.FirstOrDefault(h =>
                h.Year == dt.Year &&
                h.Month == dt.Month &&
                h.Day == dt.Day &&
                h.Hour == dt.Hour);

            var count = match?.Count ?? 0;
            var dtStart = dt;
            var dtEnd   = dtStart.AddHours(1).AddTicks(-1);

            return new GetCountPerHourResponseModel
            {
                StartDate    = dtStart.ToString("o"),
                EndDate      = dtEnd  .ToString("o"),
                ShipId       = shipId,
                CountPerHour = count
            };
        });

        return Ok(result);
    }
}