using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WinUICommunity;
public static partial class JsonUtil
{
    private readonly static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        }
    };

    public static string Serialize(object obj, bool indented = false)
    {
        var formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(obj, formatting, jsonSerializerSettings);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
    }

    public static void Populate(string json, object value)
    {
        JsonConvert.PopulateObject(json, value, jsonSerializerSettings);
    }
}
