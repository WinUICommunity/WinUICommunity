using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml.Controls.AnimatedVisuals;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;

public partial class JsonNavigationViewService : PageServiceEx, IJsonNavigationViewService
{
    private NavigationView? _navigationView;
    private AutoSuggestBox? _autoSuggestBox;
    public IList<object>? MenuItems => _navigationView?.MenuItems;
    public IList<object>? FooterMenuItems => _navigationView?.FooterMenuItems;
    private IList<object>? _menuItemsWithFooterMenuItems => MenuItems?.Concat(FooterMenuItems)?.ToList();
    public object? SettingsItem => _navigationView?.SettingsItem;

    private Dictionary<string, Type> _navigationPageDictionary = new Dictionary<string, Type>();
    private Dictionary<Type, BreadcrumbPageConfig> _breadcrumbPageDictionary;

    private Type _defaultPage { get; set; }
    private Type _settingsPage { get; set; }
    private Type _sectionPage { get; set; }
    public ResourceManager ResourceManager { get; set; }
    public ResourceContext ResourceContext { get; set; }
    public object CurrentPageParameter { get; set; }

    private string _autoSuggestBoxNotFoundString;
    private string _autoSuggestBoxNotFoundImagePath;
    public string JsonFilePath;
    private PathType _pathType;
    public DataSource DataSource { get; set; }
    private NavigationViewItem topLevelItem { get; set; }

    [MemberNotNull(nameof(_navigationView))]
    public void Initialize(NavigationView navigationView, Frame frame, Dictionary<string, Type> pages)
    {
        _navigationView = navigationView;
        this.Frame = frame;
        this._navigationPageDictionary = pages;
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
        var settingItem = (NavigationViewItem)SettingsItem;
        if (settingItem != null)
        {
            settingItem.Icon = GetAnimatedSettingsIcon();
        }

        Navigated += (s, e) =>
        {
            navigationView.IsBackEnabled = CanGoBack;

            if (e.SourcePageType == _settingsPage)
            {
                navigationView.SelectedItem = SettingsItem;
                return;
            }

            var dataItem = e.Parameter as DataItem;
            var dataGroup = e.Parameter as DataGroup;
            NavigationViewItem selectedItem = null;
            if (dataGroup == null && dataItem == null)
            {
                selectedItem = GetSelectedItem(_menuItemsWithFooterMenuItems, e.SourcePageType);
            }
            else
            {
                selectedItem = GetSelectedItem(_menuItemsWithFooterMenuItems, dataItem, dataGroup);
            }

            if (selectedItem != null)
            {
                _navigationView.SelectedItem = selectedItem;
                ExpandItems(selectedItem);
            }
        };
    }

    private IconElement GetAnimatedSettingsIcon()
    {
        var animatedIcon = new AnimatedIcon();
        animatedIcon.Source = new AnimatedSettingsVisualSource();
        animatedIcon.FallbackIconSource = new FontIconSource() { Glyph = "\uE713" };
        return animatedIcon;
    }
    private void ExpandItems(NavigationViewItem navigationViewItem)
    {
        if (navigationViewItem != null)
        {
            var parent = navigationViewItem.GetValue(NavigationHelperEx.ParentProperty) as NavigationViewItem;
            if (parent != null)
            {
                parent.IsExpanded = true;
                ExpandItems(parent);
            }
        }
    }

    private void _autoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            if (args.ChosenSuggestion is DataItem)
            {
                var infoDataItem = args.ChosenSuggestion as DataItem;
                NavigateTo(infoDataItem.UniqueId + infoDataItem.Parameter, infoDataItem);
            }
        }
    }

    private void _autoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        var matches = SearchNavigationViewItems(DataSource.Groups.SelectMany(group => group.Items), sender.Text);

        if (matches.Any())
        {
            foreach (var item in matches)
            {
                if (string.IsNullOrEmpty(item.ImagePath))
                {
                    item.ImagePath = _autoSuggestBoxNotFoundImagePath;
                }
            }
            _autoSuggestBox.ItemsSource = matches.OrderByDescending(i => i.Title.StartsWith(sender.Text.ToLowerInvariant())).ThenBy(i => i.Title);
        }
        else
        {
            var noResultsItem = new DataItem(_autoSuggestBoxNotFoundString, _autoSuggestBoxNotFoundImagePath);
            var noResultsList = new List<DataItem>();
            noResultsList.Add(noResultsItem);
            _autoSuggestBox.ItemsSource = noResultsList;
        }
    }

    public IEnumerable<DataItem> SearchNavigationViewItems(IEnumerable<DataItem> items, string query)
    {
        query = query.Trim()?.ToLowerInvariant();

        foreach (var item in items)
        {
            if (item.Title.ToLowerInvariant().Contains(query) && item.UniqueId != null)
            {
                yield return item;
            }

            if (item.Items != null && item.Items.Any())
            {
                var nestedMatches = SearchNavigationViewItems(item.Items, query);
                foreach (var nestedMatch in nestedMatches)
                {
                    yield return nestedMatch;
                }
            }
        }
    }
    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => GoBack();

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            if (GetPageType(SettingsPageKey) != null)
            {
                string pageTitle = string.Empty;
                var item = SettingsItem as NavigationViewItem;
                if (item != null && item.Content != null)
                {
                    pageTitle = item.Content?.ToString();
                }
                NavigateTo(SettingsPageKey, pageTitle);
            }
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (_sectionPage != null && selectedItem.DataContext is DataGroup itemGroup && !string.IsNullOrEmpty(itemGroup.SectionId))
            {
                NavigateTo(SectionPageKey, itemGroup);
            }
            else if (_sectionPage != null && selectedItem.DataContext is DataItem item && !string.IsNullOrEmpty(item.SectionId) && item.Items.Count > 0)
            {
                NavigateTo(SectionPageKey, item);
            }
            else
            {
                if (selectedItem?.GetValue(NavigationHelperEx.NavigateToProperty) is string pageKey)
                {
                    var dataItem = selectedItem?.DataContext as DataItem;
                    NavigateTo(pageKey, dataItem);
                }
            }
        }
    }

    public NavigationViewItem GetSelectedItem(NavigationViewItem navigationViewItem)
    {
        var rootItem = navigationViewItem?.GetValue(NavigationHelperEx.NavigateToProperty) as string;

        if (!string.IsNullOrEmpty(rootItem))
        {
            foreach (var baseItem in _menuItemsWithFooterMenuItems)
            {
                if (baseItem is NavigationViewItem item)
                {
                    var subItem = item?.GetValue(NavigationHelperEx.NavigateToProperty) as string;

                    if (rootItem.Equals(subItem))
                    {
                        return item;
                    }
                    if (item.MenuItems.Count > 0)
                    {
                        GetSelectedItem(item);
                    }
                }
            }
        }
        return null;
    }

    public NavigationViewItem GetSelectedItem(IList<object> items, DataItem dataItem, DataGroup dataGroup)
    {
        if (dataItem != null)
        {
            var rootItem = dataItem.UniqueId + dataItem.Parameter?.ToString();
            if (!string.IsNullOrEmpty(rootItem))
            {
                foreach (var baseItem in items)
                {
                    if (baseItem is NavigationViewItem item)
                    {
                        var subItem = item.GetValue(NavigationHelperEx.NavigateToProperty) as string;

                        if (rootItem.Equals(subItem))
                        {
                            return item;
                        }

                        if (item.MenuItems.Any())
                        {
                            var selectedItem = GetSelectedItem(item.MenuItems, dataItem, null);
                            if (selectedItem != null)
                            {
                                return selectedItem;
                            }
                        }
                    }
                }
            }
        }

        if (dataGroup != null)
        {
            var rootItem = dataGroup.UniqueId;
            if (!string.IsNullOrEmpty(rootItem))
            {
                foreach (var baseItem in items)
                {
                    if (baseItem is NavigationViewItem item)
                    {
                        var subItem = item.GetValue(NavigationHelperEx.NavigateToProperty) as string;

                        if (rootItem.Equals(subItem))
                        {
                            return item;
                        }

                        if (item.MenuItems.Any())
                        {
                            var selectedItem = GetSelectedItem(item.MenuItems, null, dataGroup);
                            if (selectedItem != null)
                            {
                                return selectedItem;
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public NavigationViewItem GetSelectedItem(IList<object> items, Type currentPageType)
    {
        foreach (var item in items)
        {
            if (item is NavigationViewItem navigationViewItem)
            {
                string navigatedToValue = NavigationHelperEx.GetNavigateTo(navigationViewItem);
                var pageType = GetPageType(navigatedToValue);
                if (pageType == currentPageType)
                {
                    return navigationViewItem;
                }

                if (navigationViewItem.MenuItems.Any())
                {
                    var selectedItem = GetSelectedItem(navigationViewItem.MenuItems, currentPageType);
                    if (selectedItem != null)
                        return selectedItem;
                }
            }
        }

        return null;
    }
}
