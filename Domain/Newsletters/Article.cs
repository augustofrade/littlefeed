namespace LittleFeed.Domain.Newsletters;

public class Article : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid NewsletterId { get; private set; }

    public void SetNewsletter(Newsletter newsletter)
    {
        NewsletterId = newsletter.Id;
    }
}
