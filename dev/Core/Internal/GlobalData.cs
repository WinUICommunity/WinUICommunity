using System.Text.Json;

namespace WinUICommunity;

internal class GlobalData
{
    public static void Init()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                var json = File.ReadAllText(SavePath);
                Config = (string.IsNullOrEmpty(json) ? new CoreSettings() : JsonSerializer.Deserialize<CoreSettings>(json, GlobalDataJsonContext.Default.CoreSettings)) ?? new CoreSettings();
            }
            catch
            {
                Config = new CoreSettings();
            }
        }
        else
        {
            Config = new CoreSettings();
        }
    }

    public static void Save()
    {
        var json = JsonSerializer.Serialize(Config, GlobalDataJsonContext.Default.CoreSettings);
        File.WriteAllText(SavePath, json);
    }

    public static CoreSettings Config { get; set; }
    public static string SavePath { get; set; }

}
