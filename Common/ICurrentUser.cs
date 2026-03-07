namespace LittleFeed.Common;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? UserName { get; }
    string? DisplayName { get; }
    string? Email { get; }
}