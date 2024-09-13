using System.Text.Json.Serialization;

namespace WinUICommunity;
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(DataSource))]
internal partial class JsonNavigationContext : JsonSerializerContext
{
}
