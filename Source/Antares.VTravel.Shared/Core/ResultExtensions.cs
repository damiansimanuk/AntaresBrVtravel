namespace Antares.VTravel.Shared.Core;

public static class ResultExtensions
{
    public static Result<M> Bind<T, M>(this Result<T> result, Func<T, Result<M>> func)
    {
        return result.IsError
            ? Result.Failure<M>(result.Errors)
            : func(result.Value!);
    }

    public static async Task<Result<M>> BindAsync<T, M>(this Result<T> result, Func<T, Task<Result<M>>> func)
    {
        return result.IsError
            ? Result.Failure<M>(result.Errors)
            : await func(result.Value!);
    }

    public static Result<M> Map<T, M>(this Result<T> result, Func<T, M> func)
    {
        return result.IsError
            ? Result.Failure<M>(result.Errors)
            : func(result.Value!);
    }

    public static Result<T> Then<T>(this Result<T> result, Action<T> func)
    {
        if (!result.IsError)
        {
            func(result.Value!);
        }
        return result;
    }


    public static M Match<T, M>(this Result<T> result, Func<T, M> success, Func<IReadOnlyList<Error>, M> failure)
    {
        return result.IsSuccess
            ? success(result.Value!)
            : failure(result.Errors);
    }
}
