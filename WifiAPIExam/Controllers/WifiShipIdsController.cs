using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Controllers.Models;
using WifiAPIExam.Database;

namespace WifiAPIExam.Controllers;

[ApiController]
[Route("Wifi/ShipIds")]
public class WifiShipIdsController : ControllerBase
{
    private readonly WifiDbContext _context;

    public WifiShipIdsController(WifiDbContext context)
        => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetShipIdsResponseModel>>> GetAll()
    {
        var ids = await _context.ShipIds
            .Select(s => new GetShipIdsResponseModel { ShipId = s.ShipId })
            .ToListAsync();
        return Ok(ids);
    }
}
