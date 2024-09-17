using System.Text.Json.Serialization;

namespace WinUICommunity;
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(DataSource))]
internal partial class JsonNavigationContext : JsonSerializerContext
{
}
