namespace LittleFeed.Dto.Pagination;

public record NewsletterArticlePaginationDto<T>(ListPagination<T> Pagination, string NewsletterSlug)
{
    public bool PageHasData => Pagination.Data.Count > 0;
}