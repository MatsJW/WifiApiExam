using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Controllers.Models;
using WifiAPIExam.Database;
using WifiAPIExam.Services;

namespace WifiAPIExam.Controllers;

[ApiController]
[Route("Wifi/DataUsage/{frequency}")]
public class WifiDataUsageController : ControllerBase
{
    private readonly WifiDbContext _context;
    private readonly IAuthService _authService;
    private readonly IRolesService _roles;
    public WifiDataUsageController( WifiDbContext context, IAuthService authService, 
        IRolesService roles)
    {
        _context = context;
        _authService = authService;
        _roles = roles;
    }
    

    [HttpGet("Sum")]
    public async Task<ActionResult<IEnumerable<GetDataUsageResponseModel>>> GetSum(
        [FromRoute] string frequency,
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int? shipId = null)
    {
        if (!Enum.TryParse<Period>(frequency, true, out var p))
            return BadRequest("Invalid period");
        if (!DateTime.TryParse(startDate, out var sd) ||
            !DateTime.TryParse(endDate,   out var ed))
            return BadRequest("Invalid date format.");

        var auth = await _authService.SignedInAsync(Request);
        if (auth == null || !auth.IsSignedIn())
            return Unauthorized("You must be signed in to access this resource.");

        List<GetShipIdsResponseModel> shipIds = new();
        
        if (shipId.HasValue)
        {
            if(!await _roles.IsShipIdValidAsync(shipId.Value, auth))
                return Unauthorized("You do not have access to this ship ID.");
        } 
        else 
        {
            var shipIdsResult = await _roles.GetShipIdsAsync(auth);
            if (shipIdsResult.Result is UnauthorizedObjectResult or NotFoundObjectResult || shipIdsResult.Value == null)
                return shipIdsResult.Result;
            shipIds = shipIdsResult.Value;
        }
        
        var start  = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end    = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);
        bool hourly = p == Period.Hourly;

        var grouped = await _context.WifiDatabase
            .Where(x => (shipId == null
                            ? shipIds.Select(s => s.ShipId).Contains(x.ShipId)
                            : x.ShipId == shipId.Value)
                        && x.ActivationTime.HasValue
                        && x.ActivationTime >= start
                        && x.ActivationTime < end)
            .GroupBy(x => new {
                Year = x.ActivationTime.Value.Year,
                Month = x.ActivationTime.Value.Month,
                Day = x.ActivationTime.Value.Day,
                Hour = hourly ? x.ActivationTime.Value.Hour : 0
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                Sent     = g.Sum(x => x.DataSentKB),
                Received = g.Sum(x => x.DataReceivedKB)
            })
            .ToListAsync();

        var units = (int)(hourly ? (end - start).TotalHours : (end - start).TotalDays);
        var slots = Enumerable.Range(0, units)
            .Select(i => hourly ? start.AddHours(i) : start.AddDays(i));

        var result = slots.Select(dt =>
        {
            var m = grouped.FirstOrDefault(g =>
                g.Year  == dt.Year &&
                g.Month == dt.Month &&
                g.Day   == dt.Day &&
                g.Hour  == (hourly ? dt.Hour : 0));

            var sent     = m?.Sent     ?? 0;
            var received = m?.Received ?? 0;
            var usage    = sent + received;
            var dtEnd    = hourly 
                ? dt.AddHours(1).AddTicks(-1) 
                : dt.AddDays(1).AddTicks(-1);

            return new GetDataUsageResponseModel
            {
                StartDate    = dt.ToString("o"),
                EndDate      = dtEnd.ToString("o"),
                ShipId       = shipId ?? 0,
                DataSent     = sent,
                DataReceived = received,
                DataUsage    = usage
            };
        });

        return Ok(result);
    }

    [HttpGet("Average")]
    public async Task<ActionResult<IEnumerable<GetDataUsageResponseModel>>> GetAverage(
        [FromRoute] string frequency,
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] int? shipId = null)
    {
        if (!Enum.TryParse<Period>(frequency, true, out var p))
            return BadRequest("Invalid period");
        if (!DateTime.TryParse(startDate, out var sd) ||
            !DateTime.TryParse(endDate,   out var ed))
            return BadRequest("Invalid date format.");

        var auth = await _authService.SignedInAsync(Request);
        if (auth == null || !auth.IsSignedIn())
            return Unauthorized("You must be signed in to access this resource.");

        List<GetShipIdsResponseModel> shipIds = new();
        
        if (shipId.HasValue)
        {
            if(!await _roles.IsShipIdValidAsync(shipId.Value, auth))
                return Unauthorized("You do not have access to this ship ID.");
        } 
        else 
        {
            var shipIdsResult = await _roles.GetShipIdsAsync(auth);
            if (shipIdsResult.Result is UnauthorizedObjectResult or NotFoundObjectResult || shipIdsResult.Value == null)
                return shipIdsResult.Result;
            shipIds = shipIdsResult.Value;
        }
        
        var start  = DateTime.SpecifyKind(sd, DateTimeKind.Utc);
        var end    = DateTime.SpecifyKind(ed, DateTimeKind.Utc).Date.AddDays(1);
        bool hourly = p == Period.Hourly;

        var grouped = await _context.WifiDatabase
            .Where(x => (shipId == null
                            ? shipIds.Select(s => s.ShipId).Contains(x.ShipId)
                            : x.ShipId == shipId.Value)
                        && x.ActivationTime.HasValue
                        && x.ActivationTime >= start
                        && x.ActivationTime <  end)
            .GroupBy(x => new {
                Year = x.ActivationTime.Value.Year,
                Month = x.ActivationTime.Value.Month,
                Day = x.ActivationTime.Value.Day,
                Hour = hourly ? x.ActivationTime.Value.Hour : 0
            })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.Day,
                g.Key.Hour,
                SentAvg     = g.Average(x => x.DataSentKB),
                ReceivedAvg = g.Average(x => x.DataReceivedKB)
            })
            .ToListAsync();

        var units = (int)(hourly ? (end - start).TotalHours : (end - start).TotalDays);
        var slots = Enumerable.Range(0, units)
            .Select(i => hourly ? start.AddHours(i) : start.AddDays(i));

        var result = slots.Select(dt =>
        {
            var m = grouped.FirstOrDefault(g =>
                g.Year  == dt.Year &&
                g.Month == dt.Month &&
                g.Day   == dt.Day &&
                g.Hour  == (hourly ? dt.Hour : 0));

            var sentAvg     = m?.SentAvg     ?? 0.0;
            var receivedAvg = m?.ReceivedAvg ?? 0.0;
            var usageAvg    = sentAvg + receivedAvg;
            var dtEnd       = hourly 
                ? dt.AddHours(1).AddTicks(-1) 
                : dt.AddDays(1).AddTicks(-1);

            return new GetDataUsageResponseModel
            {
                StartDate    = dt.ToString("o"),
                EndDate      = dtEnd.ToString("o"),
                ShipId       = shipId ?? 0,
                DataSent     = (int)Math.Round(sentAvg),
                DataReceived = (int)Math.Round(receivedAvg),
                DataUsage    = (int)Math.Round(usageAvg)
            };
        });

        return Ok(result);
    }
}

