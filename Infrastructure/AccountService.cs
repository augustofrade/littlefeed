using System.Security.Claims;
using LittleFeed.Application.Accounts;
using LittleFeed.Domain;
using LittleFeed.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Infrastructure;

public sealed class AccountService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext dbContext) : IAccountService
{
    private const bool IsAuthPersistent = true;
    
    public async Task<RegisterResult> RegisterAsync(string email, string userName, string password)
    {
        userName = userName.Trim();
        email = email.Trim();
        password = password.Trim();
        
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

        await dbContext.UserProfiles.AddAsync(new UserProfile
        {
            UserId = newUser.Id,
            DisplayName = userName
        });
        await  dbContext.SaveChangesAsync(); 

        await SignInWithClaimsAsync(newUser, userName);
        
        return RegisterResult.Success();
    }
    
    public async Task<LoginResult> SignInAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return LoginResult.NotFound();
        
        var signinResult = await signInManager.CheckPasswordSignInAsync(user, password, false);

        if (signinResult.IsLockedOut)
            return LoginResult.LockedOut();
        
        if (signinResult.IsNotAllowed)
            return LoginResult.RequiresEmailConfirmation();

        if (!signinResult.Succeeded)
            return LoginResult.InvalidCredentials();

        var userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.UserId == user.Id);
        if (userProfile == null)
            return LoginResult.NotFound();
        
        await SignInWithClaimsAsync(user, userProfile.DisplayName);
        
        return LoginResult.Success();
    }

    public Task SignOutAsync()
    {
        return  signInManager.SignOutAsync();
    }

    private Task SignInWithClaimsAsync(ApplicationUser user, string displayName)
    {
        var claims = new List<Claim>
        {
            new Claim("display_name", displayName),
        };
        
        return signInManager.SignInWithClaimsAsync(user, isPersistent: IsAuthPersistent, claims);
    }
}