using System.Security.Claims;

namespace Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
    {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (claim is not null && Guid.TryParse(claim.Value, out userId)) 
            return true;
        userId = Guid.Empty;
        return false;
    }
}