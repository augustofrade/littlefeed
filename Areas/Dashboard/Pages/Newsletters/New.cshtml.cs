using LittleFeed.Dto.Newsletters;
using LittleFeed.Infrastructure.Identity;
using LittleFeed.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters;

public class New(INewsletterService newsletterService,
    UserManager<ApplicationUser>  userManager,
    ILogger<New> logger) : PageModel
{
    [BindProperty]
    public CreateNewsletterDto Input { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        var currentUserId = userManager.GetUserId(User);
        
        logger.LogInformation("Received Newsletter POST request");
        var newsletter = await newsletterService.CreateNewsletter(Input, currentUserId!);
        return RedirectToPage("/Details",  new { area = "Newsletter", slug = newsletter.Slug });
    }
}