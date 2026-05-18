using System.Security.Claims;

namespace Vega.CartService.Auth;

public static class CurrentUser
{
    public static Guid? GetId(ClaimsPrincipal principal)
    {
        var sub = principal.FindFirstValue("sub") ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}
