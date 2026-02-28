using System.ComponentModel.DataAnnotations;
using LittleFeed.Application.Accounts;
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

public class Register(IAccountService accountService) : PageModel
{
    [BindProperty]
    public CreateAccountDto Input { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<PartialViewResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var errors =  ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return RegisterError(errors.ToList());
        }
        
        var registerResult = await accountService.RegisterAsync(Input.Email, Input.Password);
        if(!registerResult.Succeeded)
            return RegisterError(registerResult.Errors.ToList());
        
        return Partial("_RegisterSuccess");
    }

    private PartialViewResult RegisterError(List<string> errors)
    {
        return Partial("_RegisterError", errors);
    }
}