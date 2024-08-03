using System.Collections.Generic;

namespace Antares.VTravel.Shared.Core;


public class Result<T>
{
    public T? Value { get; init; }
    public Error[] Errors { get; init; } = [];
    public Error? FirstError => IsError ? Errors[0] : null;
    public bool IsOk { get; init; }
    public bool IsError { get; init; }

    public Result() { }

    public Result(T? value, params Error[] errors)
    {
        Value = value;

        if (errors is not null && errors.Length > 0)
        {
            IsError = true;
            Errors = errors.ToArray();
        }
        else
        {
            IsOk = true;
        }
    }

    public static Result<T> Ok(T v)
    {
        return new(v);
    }

    public static Result<T> Error(string code, string message, string? detail = null)
    {
        var error = new Error(code, message, detail);
        return new(default!, error);
    }

    public static Result<T> Exception(Exception e)
    {
        var code = e.GetType().Name;
        var error = new Error(code, e.Message, e.StackTrace);
        return new(default, error);
    }

    public static implicit operator Result<T>(T v) => Ok(v);
    public static implicit operator Result<T>(Error e) => new(default, e);
    public static implicit operator Result<T>(Error[] errors) => new(default, errors);
    public static implicit operator Result<T>(Exception e) => Exception(e);
    public static implicit operator Result<T>(ErrorBuilder eb) => new(default, eb.GetErrors());

    public R Match<R>(Func<T, R> success, Func<IReadOnlyList<Error>, R> failure) => IsOk ? success(Value!) : failure(Errors);
}

