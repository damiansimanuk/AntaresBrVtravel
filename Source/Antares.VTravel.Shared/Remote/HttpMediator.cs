namespace Antares.VTravel.Shared.Remote;

using MediatR;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;
using Antares.VTravel.Shared.ResultFluent;

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
}
