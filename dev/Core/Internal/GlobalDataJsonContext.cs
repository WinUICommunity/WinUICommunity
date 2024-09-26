using System.Text.Json.Serialization;

namespace WinUICommunity;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(CoreSettings))]
internal partial class GlobalDataJsonContext : JsonSerializerContext
{
}
