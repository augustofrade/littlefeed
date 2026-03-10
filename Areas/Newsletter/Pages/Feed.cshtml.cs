using LittleFeed.Services.Syndication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages;

public class Feed(NewsletterSyndication newsletterSyndication) : PageModel
{
    public async Task<IActionResult> OnGet(string slug, CancellationToken ct)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        var result = await newsletterSyndication.GetFeedAsync(slug, baseUrl, ct);
        if (!result.IsSuccess)
            return RedirectToPage("/Index");

        Response.Headers["Content-Disposition"] = "inline";
        return File(result.Data.FileStream, $"application/atom+xml; charset=utf-8");
    }
}