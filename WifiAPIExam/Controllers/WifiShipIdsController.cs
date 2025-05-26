using System.Text.Json;
using Clerk.BackendAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public WifiShipIdsController(WifiDbContext context, IAuthService authService, IOptions<AuthConfiguration> authConfig)
    {
        _context = context;
        _authService = authService;
        _authConfig = authConfig;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetShipIdsResponseModel>>> GetAll()
    {
        var requestState = await _authService.SignedInAsync(Request);
        
        if (requestState == null || !requestState.IsSignedIn())
            return Unauthorized("You must be signed in to access this resource.");
        
        var sdk = new ClerkBackendApi(bearerAuth: _authConfig.Value.SecretKey);
        var orgId = requestState.Claims?.Claims.FirstOrDefault(c => c.Value.StartsWith("org_"))?.Value;

        var org = await sdk.Organizations.GetAsync(orgId);
        if (org.Organization == null)
            return NotFound("Organization not found.");

        var metadata = org.Organization.PrivateMetadata;
        if (!metadata.TryGetValue("shipIds", out var ids))
        {
            return NotFound("No ship IDs found in organization metadata.");
        }
       
        var idList = ((List<object>)ids).Select(i => new GetShipIdsResponseModel{ ShipId = Convert.ToInt32(i) }).ToList();
        return idList;
    }
}
