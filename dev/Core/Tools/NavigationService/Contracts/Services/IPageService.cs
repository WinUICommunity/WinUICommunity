namespace WindowUI;

public interface IPageService
{
    Type GetPageType(string key);

    string DefaultPageKey { get; set; }
    string SettingsPageKey { get; set; }

    void SetDefaultPage(Type pageType);

    void SetSettingsPage(Type pageType);
}
