using System.ComponentModel.DataAnnotations;
using LittleFeed.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Account.Pages;

public record CreateAccountDto
{
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class Register(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : PageModel
{
    [BindProperty]
    public CreateAccountDto Input { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<PartialViewResult> OnPostAsync()
    {
        var newUser = ApplicationUser.CreateWithEmail(Input.Email);
        var passwordValidation = await PasswordValidation.ValidateAsync(userManager, newUser, Input.Password);
        if (!passwordValidation.Succeeded)
        {
            foreach(var passwordValidationError in passwordValidation.Errors)
            {
                ModelState.AddModelError(passwordValidationError.Code, passwordValidationError.Description);
            }
        }
        
        if (!ModelState.IsValid)
        {
            var errors =  ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return RegisterError(errors);
        }
        
        var registerResult = await userManager.CreateAsync(newUser, Input.Password);
        if (!registerResult.Succeeded)
            return RegisterError(registerResult.Errors.Select(e => e.Description));
        
        return Partial("_RegisterSuccess");
    }

    private PartialViewResult RegisterError(IEnumerable<string> errors)
    {
        return Partial("_RegisterError", errors.ToList());
    }
}