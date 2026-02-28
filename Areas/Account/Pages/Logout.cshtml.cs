using LittleFeed.Application.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Account.Pages;

public class Logout(IAccountService accountService) : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Account/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await accountService.SignOutAsync();
        Response.Headers["HX-Redirect"] = Url.Content("~/");
        return new EmptyResult();
    }
}