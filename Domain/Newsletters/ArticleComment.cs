namespace LittleFeed.Domain.Newsletters;

public class ArticleComment : Entity
{
    public string OwnerId { get; private init; }
    public Guid OwnerProfileId { get; private init; }
    public UserProfile OwnerProfile { get; init; }
    public Guid ArticleId { get; private init; }
    public Article Article { get; private init; }
    public ArticleComment? ParentComment { get; init; }
    public Guid? ParentCommentId { get; init; }
    public string Comment { get; set; }

    public ICollection<ArticleComment> ChildComments { get; init; } = [];
    
    private ArticleComment()  { }

    public static ArticleComment Create(string comment, UserProfile ownerProfile, Guid articleId, Guid? parentCommentId = null)
    {
        return new ArticleComment
        {
            Comment = comment,
            OwnerId = ownerProfile.UserId,
            OwnerProfileId = ownerProfile.Id,
            ArticleId = articleId,
            ParentCommentId =  parentCommentId
        };
    }
}