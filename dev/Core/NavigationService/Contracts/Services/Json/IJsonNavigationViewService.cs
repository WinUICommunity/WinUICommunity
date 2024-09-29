using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;

public interface IJsonNavigationViewService
{
    void Initialize(NavigationView navigationView, Frame frame);
    void ConfigJson(string jsonFilePath, bool autoIncludedInBuild = false, PathType pathType = PathType.Relative);
    void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, bool useItemTemplate = true, string autoSuggestBoxNotFoundString = null, string autoSuggestBoxNotFoundImagePath = null);
    void ConfigDefaultPage(Type pageType);
    void ConfigSettingsPage(Type pageType);
    void ConfigSectionPage(Type pageType);
    void ConfigLocalizer(ResourceManager resourceManager, ResourceContext resourceContext);
    void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar);
    void UnregisterEvents();

    event NavigatedEventHandler Navigated;

    IList<object>? MenuItems { get; }
    object? SettingsItem { get; }
    DataSource DataSource { get; set; }
    NavigationViewItem? GetSelectedItem(NavigationViewItem navigationViewItem);
    NavigationViewItem? GetSelectedItem(IList<object> items, DataItem dataItem, DataGroup dataGroup);
    NavigationViewItem? GetSelectedItem(IList<object> items, Type currentPageType);
    IEnumerable<DataItem> SearchNavigationViewItems(IEnumerable<DataItem> items, string query);

    object CurrentPageParameter { get; set; }
    bool CanGoBack { get; }
    Frame? Frame { get; set; }
    Window? Window { get; set; }
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null);
    bool NavigateTo(Type pageType, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null);
    bool GoBack();
    void NavigateFromBreadcrumb(Type TargetPageType, int BreadcrumbBarIndex, bool NavigatingBackwardsFromBreadcrumb = true);
}
