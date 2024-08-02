namespace Antares.VTravel.Shared.Core;

using Antares.VTravel.Shared.Request;
using MediatR;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

public record MediatorPostDto(string TypeName);

public record MediatorPostValueDto(string TypeName, string Content);

public class HttpMediator(HttpClient http)
{
    public static string EndpointName { get; } = "HttpMediator/Invoke";

    static List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).ToList();
    static HttpStatusCode[] successStatus = [HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted];
    static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
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
        var tour = await Send(new CreateToursRequest("lala", "nada"));

        tour.Match(t => t.CreatedAt, e => throw new Exception());
    }
}