using LittleFeed.Dto.Newsletters;
using LittleFeed.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public class Index(INewsletterService newsletterService) : PageModel
{
    public List<ListNewsletterDto> NewsletterList { get; private set; } = [];
    
    public async Task OnGetAsync()
    {
        NewsletterList = await newsletterService.GetNewsletters();
    }
}