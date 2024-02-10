using System.Security.Claims;

namespace backend.Controllers;

public static class Helpers
{
    public static string GetUsername(this ClaimsPrincipal principal)
    {
        return principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
    }
}