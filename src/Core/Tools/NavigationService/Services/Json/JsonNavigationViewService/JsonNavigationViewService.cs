using System.Diagnostics.CodeAnalysis;

using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;

public partial class JsonNavigationViewService : IJsonNavigationViewService
{
    private NavigationView? _navigationView;
    private AutoSuggestBox? _autoSuggestBox;

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    private readonly JsonPageService _pageService = new();

    private Type _defaultPage { get; set; }
    private Type _settingsPage { get; set; }
    private Type _sectionPage { get; set; }
    public ILocalizer Localizer { get; set; }
    public ResourceLoader ResourceLoader { get; set; }

    private string _autoSuggestBoxNotFoundString;
    private string _autoSuggestBoxNotFoundImagePath;
    public string JsonFilePath;
    private PathType _pathType;
    private bool _autoIncludedInBuild;

    public DataSource DataSource { get; set; }

    private NavigationViewItem topLevelItem { get; set; }

    [MemberNotNull(nameof(_navigationView))]
    public void Initialize(NavigationView navigationView, Frame frame)
    {
        _navigationView = navigationView;
        this.Frame = frame;

        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
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
                selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType);
            }
            else
            {
                selectedItem = GetSelectedItem(_navigationView.MenuItems, dataItem, dataGroup);
            }

            if (selectedItem != null)
            {
                _navigationView.SelectedItem = selectedItem;
                ExpandItems(selectedItem);
            }
        };
    }

    private void ExpandItems(NavigationViewItem navigationViewItem)
    {
        if (navigationViewItem != null)
        {
            var parent = navigationViewItem.GetValue(NavigationHelper.ParentProperty) as NavigationViewItem;
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
                NavigateTo(infoDataItem.UniqueId, infoDataItem);
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

    public void ConfigSectionPage(Type sectionPage)
    {
        _sectionPage = sectionPage;
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
            if (_pageService.GetPageType(_pageService.SettingsPageKey) != null)
            {
                NavigateTo(_pageService.SettingsPageKey);
            }
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (_sectionPage != null && selectedItem.DataContext is DataGroup itemGroup && !string.IsNullOrEmpty(itemGroup.SectionId))
            {
                NavigateTo(_pageService.SectionPageKey, itemGroup);
            }
            else if (_sectionPage != null && selectedItem.DataContext is DataItem item && !string.IsNullOrEmpty(item.SectionId) && item.Items.Count > 0)
            {
                NavigateTo(_pageService.SectionPageKey, item);
            }
            else
            {
                if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
                {
                    var dataItem = selectedItem?.DataContext as DataItem;
                    NavigateTo(pageKey, dataItem);
                }
            }
        }
    }

    public NavigationViewItem GetSelectedItem(NavigationViewItem navigationViewItem)
    {
        var rootItem = navigationViewItem?.GetValue(NavigationHelper.NavigateToProperty) as string;

        if (!string.IsNullOrEmpty(rootItem))
        {
            foreach (NavigationViewItem item in _navigationView.MenuItems)
            {
                var subItem = item?.GetValue(NavigationHelper.NavigateToProperty) as string;

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
        return null;
    }

    public NavigationViewItem GetSelectedItem(IList<object> items, DataItem dataItem, DataGroup dataGroup)
    {
        if (dataItem != null)
        {
            var rootItem = dataItem.UniqueId + dataItem.Parameter?.ToString();
            if (!string.IsNullOrEmpty(rootItem))
            {
                foreach (NavigationViewItem item in items)
                {
                    var subItem = item.GetValue(NavigationHelper.NavigateToProperty) as string;

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

        if (dataGroup != null)
        {
            var rootItem = dataGroup.UniqueId;
            if (!string.IsNullOrEmpty(rootItem))
            {
                foreach (NavigationViewItem item in items)
                {
                    var subItem = item.GetValue(NavigationHelper.NavigateToProperty) as string;

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

        return null;
    }

    public NavigationViewItem GetSelectedItem(IList<object> items, Type currentPageType)
    {
        foreach (var item in items)
        {
            if (item is NavigationViewItem navigationViewItem)
            {
                string navigatedToValue = NavigationHelper.GetNavigateTo(navigationViewItem);
                var pageType = _pageService.GetPageType(navigatedToValue);
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
