using Microsoft.AspNetCore.Identity;

namespace LittleFeed.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public ApplicationUser(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty("Email is required.", nameof(email));
        
        Email = email;
        UserName = email;
    }
    
    public static ApplicationUser CreateWithEmail(string email) => new(email);
}