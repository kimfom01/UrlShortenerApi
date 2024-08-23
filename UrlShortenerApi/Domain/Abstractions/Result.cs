namespace UrlShortenerApi.Domain.Abstractions;

public class Result<T>
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    private readonly T? _value;

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("There can't be a value for failure");
            }

            return _value!;
        }

        private init => _value = value;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    private Result(T value)
    {
        IsSuccess = true;
        Error = Error.None;
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }
}