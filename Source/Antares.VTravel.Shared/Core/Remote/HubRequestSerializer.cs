namespace Antares.VTravel.Shared.Core.Remote;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class HubRequestSerializer
{
    static JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    static List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies().Where(t => t.FullName!.Contains(nameof(VTravel))).SelectMany(t => t.GetTypes()).ToList();

    static public object Deserialize(JsonElement request)
    {
        var typeName = request.GetProperty("TypeFullName").GetString()!;
        var type = Type.GetType(typeName) ?? allTypes.FirstOrDefault(t => t.FullName == typeName)!;
        return request.GetProperty("Content").Deserialize(type, jsonSerializerOptions)!;
    }

    static public ContentWrapper Wrap(object content)
    {
        return new ContentWrapper(content);
    }

    public record ContentWrapper
    {
        [JsonPropertyName("Content")] public object Content { get; set; }
        [JsonPropertyName("TypeFullName")] public string TypeFullName { get; set; } = null!;

        public ContentWrapper(object content)
        {
            Content = content;
            TypeFullName = content?.GetType().FullName ?? "None";
        }
    }
}
