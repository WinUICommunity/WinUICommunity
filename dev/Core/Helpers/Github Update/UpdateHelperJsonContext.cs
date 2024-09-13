using System.Text.Json.Serialization;

namespace WinUICommunity;

// Todo: Check Naming Policy
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(UpdateInfo))]
internal partial class UpdateHelperJsonContext : JsonSerializerContext
{
}
