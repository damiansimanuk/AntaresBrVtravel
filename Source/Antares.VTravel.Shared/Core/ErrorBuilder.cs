namespace Antares.VTravel.Shared.Core;

public class ErrorBuilder
{
    private List<Error> errors = new();

    public static ErrorBuilder Create() => new();

    public ErrorBuilder Add(string code, string message, string? detail = null)
    {
        errors.Add(new Error(code, message, detail));
        return this;
    }

    public ErrorBuilder Add(Error error)
    {
        errors.Add(error);
        return this;
    }

    public ErrorBuilder Add(Exception e)
    {
        var code = e.GetType().Name;
        return Add(code, e.Message, e.StackTrace);
    }

    public bool HasError => errors.Count > 0;

    public Error[] GetErrors() => errors.ToArray();
}


public static class ErrorBuilderExtensions
{
    public static ErrorBuilder Validate(this ErrorBuilder builder, Func<bool> validator, Func<Error> error)
    {
        return !validator() ? builder.Add(error()) : builder;
    }

    public static ErrorBuilder Add(this ErrorBuilder builder, bool isError, Func<Error> error)
    {
        return isError ? builder.Add(error()) : builder;
    }

    public static ErrorBuilder Add(this ErrorBuilder builder, bool isError, Error error)
    {
        return isError ? builder.Add(error) : builder;
    }

    public static Result<bool> ToResult(this ErrorBuilder builder)
    {
        return builder.HasError ? builder.GetErrors() : true;
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, T successValue)
    {
        return builder.HasError ? builder.GetErrors() : successValue;
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, Func<T> successProcessor)
    {
        return builder.HasError ? builder.GetErrors() : successProcessor();
    }

    public static async Task<Result<T>> ToResultAsync<T>(this ErrorBuilder builder, Func<Task<T>> successProcessor)
    {
        return builder.HasError ? builder.GetErrors() : await successProcessor();
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, Func<Result<T>> successProcessor)
    {
        return builder.HasError ? builder.GetErrors() : successProcessor();
    }

    public static async Task<Result<T>> ToResultAsync<T>(this ErrorBuilder builder, Func<Task<Result<T>>> successProcessor)
    {
        return builder.HasError ? builder.GetErrors() : await successProcessor();
    }
}