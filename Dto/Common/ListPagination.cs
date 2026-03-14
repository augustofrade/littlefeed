namespace LittleFeed.Dto.Common;

public record ListPagination<T>(List<T> Data, int Amount, int Page);