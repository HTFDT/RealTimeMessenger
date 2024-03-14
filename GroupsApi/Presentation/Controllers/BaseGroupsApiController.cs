using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public abstract class BaseGroupsApiController : Controller
{
    protected const string UnauthorizedMessage = "Can't get requester id from token";
    
    protected bool TryGetUserIdFromClaims(out Guid userId)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (claim is not null && Guid.TryParse(claim.Value, out userId)) 
            return true;
        userId = Guid.Empty;
        return false;
    }
}