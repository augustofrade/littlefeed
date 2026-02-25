namespace LittleFeed.Domain.Newsletters;

public class Newsletter : Entity
{
    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; private init; } = [];

    private Newsletter() { }

    public Newsletter(string name, string? slug, string description)
    {
        Name = name;
        Slug = slug ?? name.ToLower().Replace(' ', '-');
        Description = description;
    }

    public void AddArticle(Article article)
    {
        article.SetNewsletter(this);
        Articles.Add(article);
    }

    public void RemoveArticle(Article article) {
        Articles.Remove(article);
    }
}
