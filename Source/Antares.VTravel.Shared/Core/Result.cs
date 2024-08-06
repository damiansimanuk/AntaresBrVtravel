﻿using System.Collections.Generic;

namespace Antares.VTravel.Shared.Core;


public static class Result
{
    public static Result<T> Success<T>(T v) => new(v);

    public static Result<T> Error<T>(string code, string message, string? detail = null) => new(default, new Error(code, message, detail));

    public static Result<T> Failure<T>(params Error[] errors) => new(default!, errors);

    public static Result<T> Exception<T>(Exception e)
    {
        var code = e.GetType().Name;
        var error = new Error(code, e.Message, e.StackTrace);
        return new(default, error);
    }
}

public class Result<T>
{
    public T? Value { get; init; }
    public Error[] Errors { get; init; } = [];
    public Error? FirstError => IsError ? Errors[0] : null;
    public bool IsSuccess { get; init; }
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
            IsSuccess = true;
        }
    }

    public static implicit operator Result<T>(T v) => Result.Success(v);
    public static implicit operator Result<T>(Error e) => Result.Failure<T>(e);
    public static implicit operator Result<T>(Error[] errors) => Result.Failure<T>(errors);
    public static implicit operator Result<T>(Exception e) => Result.Exception<T>(e); 
}
