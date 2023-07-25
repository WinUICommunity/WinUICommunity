namespace WinUICommunity;

public interface IJsonNavigationViewService
{
    void Initialize(NavigationView navigationView, Frame frame);
    void ConfigJson(string jsonFilePath, bool autoIncludedInBuild = false, PathType pathType = PathType.Relative);
    void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, string notFoundString = null);
    void ConfigDefaultPage(Type pageType);
    void ConfigSettingsPage(Type pageType);
    void UnregisterEvents();

    event NavigatedEventHandler Navigated;

    IList<object>? MenuItems { get; }
    object? SettingsItem { get; }
    DataSource DataSource { get; set; }
    NavigationViewItem? GetSelectedItem(Type pageType);
    IEnumerable<DataItem> SearchNavigationViewItems(IEnumerable<DataItem> items, string query);

    bool CanGoBack { get; }
    Frame? Frame { get; set; }
    Window? Window { get; set; }
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);
    bool GoBack();
}
