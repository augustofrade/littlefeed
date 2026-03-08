using LittleFeed.Common.Results;
using LittleFeed.Dto.Articles;

namespace LittleFeed.Application.Articles;

public interface IArticleCommands
{
    Task<Result<ArticleDto>> CreateArticleAsDraftAsync(CreateArticleDto createDto, string userId);
    Task<Result<ArticleDto>> CreatePublishedArticleAsync(CreateArticleDto createDto, string userId);
}