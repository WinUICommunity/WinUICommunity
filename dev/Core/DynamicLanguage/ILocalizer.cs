namespace WinUICommunity;

public interface ILocalizer
{
    event EventHandler<LanguageChangedEventArgs>? LanguageChanged;

    IEnumerable<string> GetAvailableLanguages();

    string GetCurrentLanguage();

    Task SetLanguage(string language);

    string GetLocalizedString(string uid);

    IEnumerable<string> GetLocalizedStrings(string uid);

    LanguageDictionary GetCurrentLanguageDictionary();

    IEnumerable<LanguageDictionary> GetLanguageDictionaries();
}