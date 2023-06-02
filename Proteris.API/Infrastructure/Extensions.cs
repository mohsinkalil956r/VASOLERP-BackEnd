using System.Security.Claims;

namespace ERP.API.Infrastructure
{
    public static class Extensions
    {
        public static string GetUsername(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value ?? string.Empty;
        }
    }
}
