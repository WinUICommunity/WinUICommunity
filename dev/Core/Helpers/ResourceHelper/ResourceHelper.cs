using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;

public class ResourceHelper : IResourceHelper
{
    public ResourceManager ResourceManager { get; set; }
    public ResourceContext ResourceContext { get; set; }

    public ResourceHelper()
    {
        Initialize();
    }

    public ResourceHelper(ResourceManager resourceManager)
    {
        Initialize(resourceManager);
    }

    public ResourceHelper(ResourceManager resourceManager, ResourceContext resourceContext)
    {
        Initialize(resourceManager, resourceContext);
    }

    public void Initialize()
    {
        ResourceManager = new ResourceManager();
        ResourceContext = ResourceManager.CreateResourceContext();
    }

    public void Initialize(ResourceManager resourceManager)
    {
        this.ResourceManager = resourceManager;
        this.ResourceContext = resourceManager.CreateResourceContext();
    }

    public void Initialize(ResourceManager resourceManager, ResourceContext resourceContext)
    {
        this.ResourceManager = resourceManager;
        this.ResourceContext = resourceContext;
    }
    
    /// <summary>
    /// Get All Resources Keys
    /// </summary>
    /// <param name="identifier">Default value is Resources</param>
    /// <returns></returns>
    public List<string> GetAllResourcesKeys(string identifier = null)
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

    public void SetLanguage(string language)
    {
        Microsoft.Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
    }

    private string GetStringBase(string key, string language, string filename)
    {
        if (!string.IsNullOrEmpty(language))
        {
            Microsoft.Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
        }
        var candidate = ResourceManager.MainResourceMap.TryGetValue($"{filename}/{key}", ResourceContext);
        return candidate != null ? candidate.ValueAsString : key;
    }

    public string GetString(string key)
    {
        return GetStringBase(key, null, "Resources");
    }
    public string GetString(string key, string language)
    {
        return GetStringBase(key, language, "Resources");
    }
    public string GetStringFromResource(string key, string filename)
    {
        return GetStringBase(key, null, filename);
    }

    public string GetStringFromResource(string key, string language, string filename)
    {
        return GetStringBase(key, language, filename);
    }
}
