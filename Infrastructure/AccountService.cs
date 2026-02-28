using LittleFeed.Application.Accounts;
using LittleFeed.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace LittleFeed.Infrastructure;

public sealed class AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAccountService
{
    private const bool IsAuthPersistent = false;
    
    public async Task<RegisterResult> RegisterAsync(string email, string password)
    {
        var newUser = ApplicationUser.CreateWithEmail(email);
        var passwordValidation = await PasswordValidation.ValidateAsync(userManager, newUser, password);
        if (!passwordValidation.Succeeded)
        {
            var errors = passwordValidation.Errors.Select(e => e.Description);
            return RegisterResult.Failure(errors.ToList());
        }
        
        var registerResult = await userManager.CreateAsync(newUser, password);
        if (!registerResult.Succeeded)
            return RegisterResult.Failure(registerResult.Errors.Select(e => e.Description).ToList());
        
        await signInManager.SignInAsync(newUser, isPersistent: IsAuthPersistent);
        
        return RegisterResult.Success();
    }
    
    public async Task<bool> SignInAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return false;
        
        var signinResult = await signInManager.PasswordSignInAsync(user, password, IsAuthPersistent, false);
        
        return signinResult.Succeeded;
    }
}