namespace Antares.VTravel.Shared.Core;

public readonly struct Result<T>
{
    public readonly T? Value;
    public readonly Error? Error;

    private Result(T value = default!, Error? error = default!)
    {
        Value = value;
        Error = error;
        IsOk = error is null || error == default;
    }

    public bool IsOk { get; init; }

    public static Result<T> Ok(T v)
    {
        return new(v);
    }

    public static Result<T> Err(string code, string message, string? detail = null)
    {
        var error = new Error(code, message, detail);
        return new(default!, error);
    }

    public static Result<T> Exception(Exception e)
    {
        var code = e.GetType().Name;
        var error = new Error(code, e.Message, e.StackTrace);
        return new(default!, error);
    }

    public static implicit operator Result<T>(T v) => Ok(v);
    public static implicit operator Result<T>(Error e) => new(default!, e);
    public static implicit operator Result<T>(Exception e) => Exception(e);

    public R Match<R>(Func<T, R> success, Func<Error, R> failure) => IsOk ? success(Value!) : failure(Error!);
}
