using System.ServiceModel.Syndication;
using System.Xml;
using LittleFeed.Application.Newsletters;
using LittleFeed.Common.Results;
using LittleFeed.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services.Syndication;

public class NewsletterSyndication(ApplicationDbContext dbContext, INewsletterQueries newsletterQueries)
{
    public async Task<Result<FileStreamResult>> GetFeedAsync(string newsletterSlug, string baseUrl, CancellationToken ct)
    {
        var newsletter = await newsletterQueries.GetNewsletterBySlug(newsletterSlug);
        if(newsletter is null)
            return Result<FileStreamResult>.Failure("Newsletter not found");

        var newsletterUrl = $"{baseUrl}/n/{newsletterSlug}";
        
        var syndicationItems = await dbContext.Articles
            .Where(a => a.NewsletterId == newsletter.Id && a.PublishDate != null)
            .OrderByDescending(a => a.PublishDate)
            .Select(a => new SyndicationItem(
                a.Title, 
                a.Excerpt,
                new Uri($"{newsletterUrl}/a/{a.Slug}"),
                    $"urn:uuid{a.Id:D}",
                a.ModifiedAt))
            .ToListAsync(ct);

        var feed = new SyndicationFeed(newsletter.Name, newsletter.Description, new Uri(newsletterUrl),
            syndicationItems)
        {
            LastUpdatedTime = syndicationItems.Count > 0
                ? syndicationItems.Max(i => i.LastUpdatedTime)
                : DateTimeOffset.UtcNow,
            Authors = { new SyndicationPerson { Name = newsletter.Author.Name} }
        };

        var settings = new XmlWriterSettings
        {
            Encoding = System.Text.Encoding.UTF8,
            Async = true,
            Indent = true,
            OmitXmlDeclaration = false
        };

        var stream = new MemoryStream();
        await using (var writer = XmlWriter.Create(stream, settings))
        {
            var formatter = new Atom10FeedFormatter(feed);
            formatter.WriteTo(writer);
            await writer.FlushAsync();
        }

        stream.Position = 0;

        return Result<FileStreamResult>.Success(new FileStreamResult(stream, "application/atom+xml; charset=utf-8"));
    }
}