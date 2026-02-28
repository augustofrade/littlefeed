using Microsoft.AspNetCore.Identity;

namespace LittleFeed.Infrastructure.Identity;

public static class PasswordValidation
{
    public static async Task<IdentityResult> ValidateAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password)
    {
        foreach (var validator in userManager.PasswordValidators)
        {
            var result = await validator.ValidateAsync(userManager, user, password);
            if (!result.Succeeded)
                return result;
        }

        return IdentityResult.Success;
    }
}