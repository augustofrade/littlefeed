using LittleFeed.Application.Newsletters;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public class Newsletters(INewsletterQueries newsletterQueries) : PageModel
{
    public List<ListNewsletterDto> NewsletterList { get; private set; } = [];
    
    public async Task OnGetAsync()
    {
        NewsletterList = await newsletterQueries.GetNewsletters();
    }
}