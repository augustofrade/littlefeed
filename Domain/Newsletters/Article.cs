using LittleFeed.Domain.Common;

namespace LittleFeed.Domain.Newsletters;

public class Article : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public required string Excerpt { get; set; }
    public required string Body { get; set; }
    public bool IsDraft { get; set; }
    public Guid NewsletterId { get; private set; }
    public Newsletter Newsletter { get; private set; }

    private Article() { }

    public static Article Create(string title, string excerpt, string body, bool isDraft, Newsletter newsletter)
    {
        return new Article
        {
            Title = title,
            Slug = Slugifier.Slugify(title),
            Excerpt = excerpt,
            Body = body,
            NewsletterId = newsletter.Id,
            Newsletter = newsletter,
            IsDraft =  isDraft,
        };
    }

    public void SetSlug(string slug)
    {
        Slug = Slugifier.Slugify(slug);
    }
}
