namespace LittleFeed.Dto.Common;

public record ListPagination<T>(List<T> Data, int PageAmount, int Page);