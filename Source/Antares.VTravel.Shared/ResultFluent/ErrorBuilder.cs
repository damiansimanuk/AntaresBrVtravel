namespace Antares.VTravel.Shared.ResultFluent;

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
