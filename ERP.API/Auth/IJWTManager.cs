using System.Security.Claims;

namespace ERP.API.Auth
{
    public interface IJWTManager
    {
        AuthTokens Authenticate(IEnumerable<Claim> claims);
    }
}
