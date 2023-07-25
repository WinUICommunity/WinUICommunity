namespace WinUICommunity;

public interface IJsonNavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    void Initialize(NavigationView navigationView, Frame frame);
    void ConfigJson(string jsonFilePath, bool autoIncludedInBuild = false, PathType pathType = PathType.Relative);
    void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, string notFoundString = null);
    void ConfigDefaultPage(Type pageType);
    void ConfigSettingsPage(Type pageType);

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);

    event NavigatedEventHandler Navigated;

    bool CanGoBack
    {
        get;
    }

    Frame? Frame
    {
        get; set;
    }

    Window? Window
    {
        get; set;
    }

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    bool GoBack();
}
