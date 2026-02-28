using LittleFeed.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Account.Pages;

public record CreateAccountDto(string Email, string Password);

public class Register(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : PageModel
{
    [BindProperty]
    public CreateAccountDto Input { get; private set; }
    
    public void OnGet()
    {
        
    }

    public PartialViewResult OnPostAsync()
    {
        // var registerResult = await userManager.CreateAsync(ApplicationUser.CreateWithEmail(Input.Email), Input.Password);
        // if(registerResult.Succeeded)
            return Partial("_RegisterSuccess");
        
        
    }
}