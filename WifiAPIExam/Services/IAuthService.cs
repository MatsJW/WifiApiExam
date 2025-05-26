using Clerk.BackendAPI.Helpers.Jwks;

namespace WifiAPIExam.Services;

public interface IAuthService
{
    public Task<RequestState?> SignedInAsync(HttpRequest request);
}   