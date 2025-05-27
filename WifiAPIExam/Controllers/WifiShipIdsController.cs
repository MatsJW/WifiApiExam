using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WifiAPIExam.Configuration;
using WifiAPIExam.Controllers.Models;
using WifiAPIExam.Database;
using WifiAPIExam.Services;

namespace WifiAPIExam.Controllers;

[ApiController]
[Route("Wifi/ShipIds")]
public class WifiShipIdsController : ControllerBase
{
    private readonly WifiDbContext _context;
    private readonly IAuthService _authService;
    private readonly IOptions<AuthConfiguration> _authConfig;
    private readonly IRolesService _roles;

    public WifiShipIdsController(WifiDbContext context, IAuthService authService, IOptions<AuthConfiguration> authConfig, IRolesService roles)
    {
        _context = context;
        _authService = authService;
        _authConfig = authConfig;
        _roles = roles;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetShipIdsResponseModel>>> GetAll()
    {
        var requestState = await _authService.SignedInAsync(Request);
        
        if (requestState == null || !requestState.IsSignedIn())
            return Unauthorized("You must be signed in to access this resource.");
        
        return await _roles.GetShipIdsAsync(requestState);
    }
}
