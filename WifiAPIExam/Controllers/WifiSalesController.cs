using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Controllers.Models;
using WifiAPIExam.Database;

namespace WifiAPIExam.Controllers;

public enum Period
{
    Hourly,
    Daily
}

[ApiController]
[Route("Wifi/Sales")]
public class WifiDataReportController : ControllerBase
{
    private readonly WifiDbContext _context;
    private readonly ILogger<WifiDataReportController> _logger;

    public WifiDataReportController(ILogger<WifiDataReportController> logger, WifiDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{period}/Sum")]
    public async Task<ActionResult<IEnumerable<GetSalesResponseModel>>> GetSum(
        [FromRoute] string period,
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int? shipId = null)
    {
        if (!Enum.TryParse<Period>(period, true, out var p))
            return BadRequest("Invalid period");

        if (!DateTime.TryParse(startDate, out var sd) ||
            !DateTime.TryParse(endDate,   out var ed))
            return BadRequest("Invalid date format.");

        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);

        var hourly = p == Period.Hourly;

        // query and group
        var grouped = await _context.WifiDatabase
            .Where(w => (shipId == null || w.ShipId == shipId.Value)
                        && w.SellTime >= start 
                        && w.SellTime < end)
            .GroupBy(w => new {
                w.SellTime.Year,
                w.SellTime.Month,
                w.SellTime.Day,
                Hour = hourly ? w.SellTime.Hour : 0
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                Sum = (int)g.Sum(x => x.Price)
            })
            .ToListAsync();

        var totalUnits = hourly
            ? (int)(end - start).TotalHours
            : (int)(end - start).TotalDays;

        var fullRange = Enumerable.Range(0, totalUnits)
            .Select(i => hourly
                ? start.AddHours(i)
                : start.AddDays(i))
            .ToList();

        var result = fullRange.Select(dt =>
        {
            var match = grouped.FirstOrDefault(x =>
                x.Year  == dt.Year &&
                x.Month == dt.Month &&
                x.Day   == dt.Day &&
                x.Hour  == dt.Hour);

            var sum = match?.Sum ?? 0;
            var dtEnd = hourly
                ? dt.AddHours(1).AddTicks(-1)
                : dt.AddDays(1).AddTicks(-1);

            return new GetSalesResponseModel
            {
                StartDate = dt.ToString("o"),
                EndDate   = dtEnd.ToString("o"),
                ShipId    = shipId ?? 0,
                Value     = sum
            };
        });

        return Ok(result);
    }

    [HttpGet("{period}/Count")]
    public async Task<ActionResult<IEnumerable<GetSalesResponseModel>>> GetCount(
        [FromRoute] string period,
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int shipId)
    {
        if (!Enum.TryParse<Period>(period, true, out var p))
            return BadRequest("Invalid period");

        if (!DateTime.TryParse(startDate, out var sd) ||
            !DateTime.TryParse(endDate,   out var ed))
            return BadRequest("Invalid date format.");

        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);

        bool hourly = p == Period.Hourly;

        var grouped = await _context.WifiDatabase
            .Where(w => w.ShipId == shipId
                        && w.SellTime >= start
                        && w.SellTime <  end)
            .GroupBy(w => new {
                w.SellTime.Year,
                w.SellTime.Month,
                w.SellTime.Day,
                Hour = hourly ? w.SellTime.Hour : 0
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                Count = g.Count()
            })
            .ToListAsync();

        var totalUnits = hourly
            ? (int)(end - start).TotalHours
            : (int)(end - start).TotalDays;

        var fullRange = Enumerable.Range(0, totalUnits)
            .Select(i => hourly
                ? start.AddHours(i)
                : start.AddDays(i))
            .ToList();

        var result = fullRange.Select(dt =>
        {
            var match = grouped.FirstOrDefault(x =>
                x.Year  == dt.Year &&
                x.Month == dt.Month &&
                x.Day   == dt.Day &&
                x.Hour  == dt.Hour);

            var count = match?.Count ?? 0;
            var dtEnd = hourly
                ? dt.AddHours(1).AddTicks(-1)
                : dt.AddDays(1).AddTicks(-1);

            return new GetSalesResponseModel()
            {
                StartDate = dt.ToString("o"),
                EndDate   = dtEnd.ToString("o"),
                ShipId    = shipId,
                Value     = count
            };
        });

        return Ok(result);
    }
    
    [HttpGet("{period}/Average")]
    public async Task<ActionResult<IEnumerable<GetSalesResponseModel>>> GetAverage(
        [FromRoute] string period,
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int? shipId = null)
    {
        if (!Enum.TryParse<Period>(period, true, out var p))
            return BadRequest("Invalid period");

        if (!DateTime.TryParse(startDate, out var sd) ||
            !DateTime.TryParse(endDate,   out var ed))
            return BadRequest("Invalid date format.");

        var start = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end   = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);

        bool hourly = p == Period.Hourly;

        var grouped = await _context.WifiDatabase
            .Where(w => (shipId == null || w.ShipId == shipId.Value)
                        && w.SellTime >= start
                        && w.SellTime < end)
            .GroupBy(w => new {
                w.SellTime.Year,
                w.SellTime.Month,
                w.SellTime.Day,
                Hour = hourly ? w.SellTime.Hour : 0
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                Avg = g.Average(x => x.Price)
            })
            .ToListAsync();

        var totalUnits = hourly
            ? (int)(end - start).TotalHours
            : (int)(end - start).TotalDays;

        var fullRange = Enumerable.Range(0, totalUnits)
            .Select(i => hourly
                ? start.AddHours(i)
                : start.AddDays(i))
            .ToList();

        var result = fullRange.Select(dt =>
        {
            var match = grouped.FirstOrDefault(x =>
                x.Year  == dt.Year &&
                x.Month == dt.Month &&
                x.Day   == dt.Day &&
                x.Hour  == dt.Hour);

            var avg = match?.Avg ?? 0m;
            var dtEnd = hourly
                ? dt.AddHours(1).AddTicks(-1)
                : dt.AddDays(1).AddTicks(-1);

            return new GetSalesResponseModel
            {
                StartDate    = dt.ToString("o"),
                EndDate      = dtEnd.ToString("o"),
                ShipId       = shipId ?? 0,
                Value        = (double)avg
            };
        });

        return Ok(result);
    }
}