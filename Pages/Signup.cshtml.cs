using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public record CreateAccountDto(string Email, string Password);

public class Signup : PageModel
{
    [BindProperty]
    public CreateAccountDto Input { get; private set; }
    
    public void OnGet()
    {
        
    }

    public PartialViewResult OnPostAsync()
    {
        return Partial("_SignupSuccess");
    }
}