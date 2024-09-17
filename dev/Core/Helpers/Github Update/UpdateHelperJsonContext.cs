using System.Text.Json.Serialization;

namespace WinUICommunity;

[JsonSourceGenerationOptions()]
[JsonSerializable(typeof(UpdateInfo))]
internal partial class UpdateHelperJsonContext : JsonSerializerContext
{
}
