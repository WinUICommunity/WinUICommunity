using System.Text.Json.Serialization;

namespace WinUICommunity;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified)]
[JsonSerializable(typeof(UpdateInfo))]
internal partial class UpdateHelperJsonContext : JsonSerializerContext
{
}
