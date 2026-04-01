namespace LittleFeed.Dto.Pagination;

public record ListPagination<T>(List<T> Data, int PageAmount, int Page, bool HasNextPage = false);