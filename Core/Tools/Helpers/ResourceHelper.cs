using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;

public static class ResourceHelper
{
    /// <summary>
    /// Get All Resources Keys
    /// </summary>
    /// <param name="identifier">Default value is Resources</param>
    /// <returns></returns>
    public static List<string> GetAllResourcesKeys(string identifier = null)
    {
        var reslist = new List<string>();

        var rmap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
        foreach (var str in rmap.Keys)
        {
            if (str.StartsWith(identifier ?? "Resources"))
            {
                reslist.Add($"/{str}");
            }
        }

        return reslist;
    }

    public static void SetLanguage(string language, string filename = null)
    {
        ResourceManager resourceManager;
        ResourceContext m_resourceContext;

        resourceManager = string.IsNullOrEmpty(filename) ? new ResourceManager() : new ResourceManager(filename);

        m_resourceContext = resourceManager.CreateResourceContext();

        m_resourceContext = resourceManager.CreateResourceContext();
        m_resourceContext.QualifierValues["Language"] = language;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
    }

    public static string GetString(string key)
    {
        return new ResourceLoader().GetString(key);
    }

    public static string GetString(string key, string filename)
    {
        return new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath(), filename).GetString(key);
    }

    public static string GetStringWithLanguage(string key, string language = "en-US")
    {
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
        return new ResourceLoader().GetString(key);
    }

    public static string GetStringWithLanguage(string key, string filename, string language = "en-US")
    {
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
        return new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath(), filename).GetString(key);
    }
}
