namespace LittleFeed.Common.Results;

public sealed class Result<T>
{
    private readonly T? _data;
    private readonly string? _error;
    public bool IsSuccess => _error == null;

    public string Error => _error ?? string.Empty;
    public T Data => _data ?? throw new InvalidOperationException("Data is null");
    
    private Result(T data)
    {
        _data = data;
    }
    
    private Result(string? error)
    {
        _error = error;
    }

    public static Result<T> Success(T data) =>  new Result<T>(data);
    
    public static Result<T> Failure(string error) =>  new Result<T>(error);
}