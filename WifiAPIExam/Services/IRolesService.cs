using Clerk.BackendAPI.Helpers.Jwks;
using Microsoft.AspNetCore.Mvc;
using WifiAPIExam.Controllers.Models;

namespace WifiAPIExam.Services;

public interface IRolesService
{
    Task<ActionResult<List<GetShipIdsResponseModel>>> GetShipIdsAsync(RequestState request);
    Task<bool> IsShipIdValidAsync(int shipId, RequestState request);
}