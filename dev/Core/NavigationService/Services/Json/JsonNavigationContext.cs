using System.Text.Json.Serialization;

namespace WinUICommunity;
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified)]
[JsonSerializable(typeof(DataSource))]
internal partial class JsonNavigationContext : JsonSerializerContext
{
}
