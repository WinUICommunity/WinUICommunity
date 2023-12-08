namespace WindowUI;

public class NullLocalizer : ILocalizer
{
    private NullLocalizer()
    {
    }

    public event EventHandler<LanguageChangedEventArgs>? LanguageChanged { add { } remove { } }

    public static ILocalizer Instance { get; } = new NullLocalizer();

    public IEnumerable<string> GetAvailableLanguages()
    {
        return Array.Empty<string>();
    }

    public string GetCurrentLanguage()
    {
        return string.Empty;
    }

    public Task SetLanguage(string language)
    {
        return Task.FromResult(false);
    }

    public string GetLocalizedString(string uid)
    {
        return uid;
    }

    public IEnumerable<string> GetLocalizedStrings(string uid)
    {
        return new string[] { uid };
    }

    public LanguageDictionary GetCurrentLanguageDictionary()
    {
        return new("");
    }

    public IEnumerable<LanguageDictionary> GetLanguageDictionaries()
    {
        return Enumerable.Empty<LanguageDictionary>();
    }
}