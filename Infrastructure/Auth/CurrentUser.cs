using System.Security.Claims;
using LittleFeed.Common;

namespace LittleFeed.Infrastructure.Auth;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        Principal?.Identity?.IsAuthenticated == true;

    public string? UserId =>
        Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName =>
        Principal?.Identity?.Name;
    
    public string? DisplayName =>
        Principal?.FindFirstValue("display_name");

    public string? Email =>
        Principal?.FindFirstValue(ClaimTypes.Email);
}