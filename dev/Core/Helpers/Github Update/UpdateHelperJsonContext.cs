using System.Text.Json.Serialization;

namespace WinUICommunity;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(UpdateInfo))]
internal partial class UpdateHelperJsonContext : JsonSerializerContext
{
}
