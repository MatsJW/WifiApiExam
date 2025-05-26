using Clerk.BackendAPI.Helpers.Jwks;
using Microsoft.Extensions.Options;
using WifiAPIExam.Configuration;

namespace WifiAPIExam.Services;

public class AuthService : IAuthService
{
    private readonly IOptions<AuthConfiguration> _auth;
    
    public AuthService(IOptions<AuthConfiguration> auth)
    {
        _auth = auth;
    }

    
    public async Task<RequestState?> SignedInAsync(HttpRequest request)
    {
        var options = new AuthenticateRequestOptions(
            secretKey: _auth.Value.SecretKey,
            authorizedParties: new string[] { "http://localhost:5173" }
        );

        var requestState = await AuthenticateRequest.AuthenticateRequestAsync(request, options);
        
        
        
        return requestState;
    }
}