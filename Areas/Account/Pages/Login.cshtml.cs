using System.ComponentModel.DataAnnotations;
using LittleFeed.Application.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LittleFeed.Application.Accounts;

namespace LittleFeed.Areas.Account.Pages;


public record LoginDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class Login(IAccountService accountService) : PageModel
{
    [BindProperty]
    public LoginDto Input { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    
    public void OnGet()
    {
        ReturnUrl ??= Url.Content("~/");
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return LoginError(LoginResult.Failure("Email and password are required"));
        }
        
        var result = await accountService.SignInAsync(Input.Email, Input.Password);
        if(!result.Succeeded)
            return LoginError(result);

        Response.Headers["HX-Redirect"] = Url.Content(ReturnUrl);
        return new EmptyResult();
    }

    private PartialViewResult LoginError(LoginResult signinResult)
    {
        return Partial("_LoginError", signinResult.Error);
    }
}