using LittleFeed.Application.Newsletters;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public class Index(INewsletterQueries newsletterQueries) : PageModel
{
    public List<ListNewsletterDto> NewsletterList { get; private set; } = [];
    
    public async Task OnGetAsync()
    {
        NewsletterList = await newsletterQueries.GetNewsletters();
    }
}