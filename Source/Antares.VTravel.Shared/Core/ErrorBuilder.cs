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

    public Result<T> ErrorOr<T>(T value)
    {
        return HasError ? GetErrors() : value;
    }

    public Result<T> ErrorOr<T>(Func<T> valueProcessor)
    {
        return HasError ? GetErrors() : valueProcessor();
    }
}


