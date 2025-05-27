using Clerk.BackendAPI;
using Clerk.BackendAPI.Helpers.Jwks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WifiAPIExam.Configuration;
using WifiAPIExam.Controllers.Models;

namespace WifiAPIExam.Services;

public class RolesService: IRolesService
{
    private readonly IAuthService _authService;
    private readonly IOptions<AuthConfiguration> _authConfig;
    
    public RolesService(IAuthService authService, IOptions<AuthConfiguration> authConfig)
    {
        _authService = authService;
        _authConfig = authConfig;
    }
    
    public async Task<ActionResult<List<GetShipIdsResponseModel>>> GetShipIdsAsync(RequestState request)
    {
        if (request == null || !request.IsSignedIn())
            return new UnauthorizedObjectResult("You must be signed in to access this resource.");

        var sdk = new ClerkBackendApi(bearerAuth: _authConfig.Value.SecretKey);
        var orgId = request.Claims?.Claims.FirstOrDefault(c => c.Value.StartsWith("org_"))?.Value;

        var org = await sdk.Organizations.GetAsync(orgId);
        if (org.Organization == null)
            return new NotFoundObjectResult("Organization not found.");

        var metadata = org.Organization.PrivateMetadata;
        if (!metadata.TryGetValue("shipIds", out var ids))
        {
            return new NotFoundObjectResult("No ship IDs found in organization metadata.");
        }

        var idList = ((List<object>)ids).Select(i => new GetShipIdsResponseModel{ ShipId = Convert.ToInt32(i) }).ToList();
        return idList;
    }
    
    public async Task<bool> IsShipIdValidAsync(int shipId, RequestState request)
    {
        var shipIds = await GetShipIdsAsync(request);
        if (shipIds.Result is UnauthorizedObjectResult or NotFoundObjectResult)
            return false;

        return shipIds.Value.Any(s => s.ShipId == shipId);
    }
}