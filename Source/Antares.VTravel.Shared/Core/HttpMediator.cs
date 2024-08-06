namespace Antares.VTravel.Shared.Core;

using Antares.VTravel.Shared.Request;
using MediatR;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Antares.VTravel.Shared.Dto;

public record MediatorPostDto(string TypeName);

public record MediatorPostValueDto(string TypeName, string Content);

public class HttpMediator(HttpClient http)
{
    public static string EndpointName = "HttpMediator/Invoke";

    static List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).ToList();
    static HttpStatusCode[] successStatus = [HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted];
    static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<TResponse>(request, cancellationToken);
    }

    public async Task<Result<TResponse>> Send<TResponse>(IRequest<Result<TResponse>> request, CancellationToken cancellationToken = default)
    {
        var result = await PostAsync<Result<TResponse>>(request, cancellationToken);
        return result.IsSuccess ? result.Value! : result.Errors!;
    }

    private async Task<Result<TResponse>> PostAsync<TResponse>(IBaseRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var jsonObject = Serialize(request);
            using var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            using var response = await http.PostAsync(EndpointName, content, cancellationToken).ConfigureAwait(false);

            if (successStatus.Contains(response.StatusCode))
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return JsonSerializer.Deserialize<TResponse>(responseContent, jsonSerializerOptions)!;
            }

            return new Error("HttpRequest", $"StatusCode: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static string Serialize(IBaseRequest request)
    {
        var typeName = request.GetType().FullName!;
        var content = JsonSerializer.Serialize((object)request);
        return JsonSerializer.Serialize(new MediatorPostValueDto(typeName, content));
    }

    public static object? Deserialize(MediatorPostValueDto req)
    {
        var type = allTypes.FirstOrDefault(t => t.FullName == req.TypeName);
        return JsonSerializer.Deserialize(req.Content, type!);
    }

    public async Task Test()
    {
        int? id = null!;
        var tour = await Send(new CreateTourRequest(false, "lala", "nada"));

        tour.Match(t => t.CreatedAt, e => throw new Exception());


        Result<int> test;

        test = ErrorBuilder.Create()
            .Add("Code", "Message", "Detail")
            .Add(new Error("Code", "Message", "Detail"))
            .GetErrors();

        var errors = ErrorBuilder.Create()
            .Add("Code", "Message", "Detail")
            .Add(new Error("Code", "Message", "Detail"))
            .Add(new Exception());

        test = errors.HasError ? errors.GetErrors() : 2;
        var test2 = errors.ToResult(33);

        Result<int> resultFromError = errors.GetErrors();
        resultFromError.Map(t => 33);

        var errors2 = test2.Errors;
        var value = test2.Value;

        var test3 = errors.ToResult(() => "nada");

        var result = Validation1(null!)
            .Bind(Validation2)
            .Bind(Validation2)
            .Bind(SendEmail)
            .Map(e => e ? 3f : 1d);


        var realResult = ValidateInput(null)
            .Bind(Validation2);
    }


    public Result<TourDto> ValidateInput(TourDto tourDto)
    {
        var errors = ErrorBuilder.Create();
        errors.Validate(() => string.IsNullOrEmpty(tourDto.Name), () => new Error("Invalid Name", "null"));
        errors.Add(string.IsNullOrEmpty(tourDto.Name), () => new Error("Invalid Name", "null"));
        errors.Add(string.IsNullOrEmpty(tourDto.Name), new Error("Invalid Name", "null"));
        return errors.ToResult(tourDto);
    }

    public Result<TourDto> Validation1(TourDto tourDto)
    {
        if (object.Equals(tourDto, null))
            return new Error("Serialize", "");

        return tourDto;
    }

    public Result<TourDto> Validation2(TourDto tourDto)
    {
        if (object.Equals(tourDto, null))
            return new Error("Serialize", "");

        return tourDto;
    }

    public Result<bool> SendEmail(TourDto tourDto)
    {
        if (object.Equals(tourDto, null))
            return new Error("Serialize", "");

        return true;
    }
}