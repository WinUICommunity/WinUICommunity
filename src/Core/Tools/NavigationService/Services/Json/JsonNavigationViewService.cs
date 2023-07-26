using Microsoft.UI.Xaml.Automation;

using System.Diagnostics.CodeAnalysis;

namespace WinUICommunity;

public class JsonNavigationViewService : IJsonNavigationViewService
{
    private NavigationView? _navigationView;
    private AutoSuggestBox? _autoSuggestBox;

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    private readonly JsonPageService _pageService = new();

    private Type _defaultPage { get; set; }
    private Type _settingsPage { get; set; }
    private Type _sectionPage { get; set; }

    #region NavigationService
    private object? _lastParameterUsed;
    private Frame? _frame;
    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = Window?.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    public Window Window { get; set; }

    #endregion

    private string _autoSuggestBoxNotFoundString;
    private string _autoSuggestBoxNotFoundImagePath;
    public string _jsonFilePath;
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
            string uniqueId = string.Empty;

            if (dataGroup != null && !string.IsNullOrEmpty(dataGroup.SectionId))
            {
                uniqueId = dataGroup.UniqueId;
            }
            else if (dataItem != null && !string.IsNullOrEmpty(dataItem.SectionId))
            {
                uniqueId = dataItem.UniqueId;
            }

            var selectedItem = GetSelectedItem(e.SourcePageType, uniqueId);
            GetParentAndExpandItem(selectedItem);
            navigationView.SelectedItem = selectedItem;
        };
    }

    private void GetParentAndExpandItem(NavigationViewItem navigationViewItem)
    {
        if (navigationViewItem != null)
        {
            var parent = navigationViewItem.GetValue(NavigationHelper.ParentProperty) as NavigationViewItem;
            if (parent != null)
            {
                parent.IsExpanded = true;
                GetParentAndExpandItem(parent);
            }
        }
    }

    public async void ConfigJson(string jsonFilePath, bool autoIncludedInBuild = false, PathType pathType = PathType.Relative)
    {
        _jsonFilePath = jsonFilePath;
        _pathType = pathType;
        _autoIncludedInBuild = autoIncludedInBuild;
        DataSource = new DataSource();

        await AddNavigationMenuItems();
    }

    public void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, bool useItemTemplate = true, string autoSuggestBoxNotFoundString = null, string autoSuggestBoxNotFoundImagePath = null)
    {
        _autoSuggestBox = autoSuggestBox;

        if (_autoSuggestBox != null)
        {
            _autoSuggestBoxNotFoundString = autoSuggestBoxNotFoundString;
            _autoSuggestBoxNotFoundImagePath = autoSuggestBoxNotFoundImagePath;

            if (string.IsNullOrEmpty(_autoSuggestBoxNotFoundString))
            {
                _autoSuggestBoxNotFoundString = "No result found";
            }

            if (string.IsNullOrEmpty(_autoSuggestBoxNotFoundImagePath))
            {
                _autoSuggestBoxNotFoundImagePath = "Assets/autoSuggestBoxNotFound.png";
            }

            _autoSuggestBox.TextChanged += _autoSuggestBox_TextChanged;
            _autoSuggestBox.QuerySubmitted += _autoSuggestBox_QuerySubmitted;

            if (useItemTemplate)
            {
                CreateAutoSuggestBoxItemTemplate();
            }
        }
    }

    private void CreateAutoSuggestBoxItemTemplate()
    {
        string dataTemplateXaml = @"
    <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
        <Grid ColumnSpacing='12'>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width='16' />
                <ColumnDefinition Width='*' />
            </Grid.ColumnDefinitions>
            <Image Source='{Binding ImagePath}' />
            <TextBlock Grid.Column='1' Text='{Binding Title}' />
        </Grid>
    </DataTemplate>";

        var dataTemplate = XamlReader.Load(dataTemplateXaml) as DataTemplate;
        _autoSuggestBox.ItemTemplate = dataTemplate;
    }

    private void _autoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            if (args.ChosenSuggestion is DataItem)
            {
                var infoDataItem = args.ChosenSuggestion as DataItem;
                NavigateTo(infoDataItem.UniqueId);
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

    public void ConfigDefaultPage(Type DefaultPage)
    {
        _defaultPage = DefaultPage;
    }

    public void ConfigSettingsPage(Type SettingsPage)
    {
        _settingsPage = SettingsPage;
    }

    private void ConfigPages()
    {
        _pageService.GetPages(MenuItems);
        _pageService.SetDefaultPage(_defaultPage);
        _pageService.SetSettingsPage(_settingsPage);
        _pageService.SetSectionPage(_sectionPage);

        if (_pageService.GetPageType(_pageService.DefaultPageKey) != null)
        {
            NavigateTo(_pageService.DefaultPageKey);
        }
    }

    public NavigationViewItem? GetSelectedItem(Type pageType, string uniqueId)
    {
        if (_navigationView != null)
        {
            return GetSelectedItem(_navigationView.MenuItems, pageType, uniqueId) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType, uniqueId);
        }

        return null;
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType, string uniqueId)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType, uniqueId))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(item.MenuItems, pageType, uniqueId);
            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType, string uniqueId)
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                return _pageService.GetPageType(pageKey) == sourcePageType;
            }
        }
        else
        {
            var dataGroup = menuItem.DataContext as DataGroup;
            var dataItem = menuItem.DataContext as DataItem;

            if (dataGroup != null && !string.IsNullOrEmpty(dataGroup.UniqueId))
            {
                if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey && dataGroup.UniqueId.Equals(uniqueId))
                {
                    return _pageService.GetPageType(pageKey) == sourcePageType;
                }
            }

            if (dataItem != null && !string.IsNullOrEmpty(dataItem.UniqueId))
            {
                if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey && dataItem.UniqueId.Equals(uniqueId))
                {
                    return _pageService.GetPageType(pageKey) == sourcePageType;
                }
            }
        }
        return false;
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
                    NavigateTo(pageKey);
                }
            }
        }
    }

    private void AddNavigationViewItemsRecursively(IEnumerable<DataItem> navItems, bool isFooterNavigationViewItem, bool hasTopLevel, string pageKey, NavigationViewItem parentNavItem = null)
    {
        foreach (var navItem in navItems)
        {
            var navigationViewItem = new NavigationViewItem()
            {
                IsEnabled = navItem.IncludedInBuild,
                Content = navItem.Title,
                Tag = navItem.UniqueId,
                DataContext = navItem
            };

            if (!string.IsNullOrEmpty(navItem.ImageIconPath))
            {
                var icon = GetIcon(navItem.ImageIconPath);
                if (icon != null)
                {
                    navigationViewItem.Icon = icon;
                }
            }

            NavigationHelper.SetNavigateTo(navigationViewItem, navItem.UniqueId);
            navigationViewItem.InfoBadge = GetInfoBadge(navItem);
            AutomationProperties.SetName(navigationViewItem, navItem.Title);

            if (parentNavItem == null)
            {
                if (hasTopLevel)
                {
                    NavigationHelper.SetParent(navigationViewItem, topLevelItem);
                    topLevelItem.MenuItems.Add(navigationViewItem);
                }
                else
                {
                    if (isFooterNavigationViewItem)
                    {
                        _navigationView.FooterMenuItems.Add(navigationViewItem);
                    }
                    else
                    {
                        _navigationView.MenuItems.Add(navigationViewItem);
                    }
                }
            }
            else
            {
                NavigationHelper.SetParent(navigationViewItem, parentNavItem);
                parentNavItem.MenuItems.Add(navigationViewItem);
            }

            if (navItem.Items != null && navItem.Items.Count > 0)
            {
                AddNavigationViewItemsRecursively(navItem.Items, isFooterNavigationViewItem, hasTopLevel, navItem.UniqueId, navigationViewItem);
            }
        }
    }

    private async Task AddNavigationMenuItems()
    {
        await DataSource.GetDataAsync(_jsonFilePath, _pathType, _autoIncludedInBuild);

        foreach (var group in DataSource.Groups.OrderBy(i => i.Title).Where(i => !i.IsSpecialSection && !i.HideGroup))
        {
            if (group.ShowItemsWithoutGroup)
            {
                var items = group.Items.Where(i => !i.HideNavigationViewItem);
                AddNavigationViewItemsRecursively(items, group.IsFooterNavigationViewItem, false, group.UniqueId);
            }
            else
            {
                topLevelItem = new NavigationViewItem()
                {
                    Content = group.Title,
                    IsExpanded = group.IsExpanded,
                    Tag = group.UniqueId,
                    DataContext = group
                };

                if (!string.IsNullOrEmpty(group.ImageIconPath))
                {
                    var icon = GetIcon(group.ImageIconPath);
                    if (icon != null)
                    {
                        topLevelItem.Icon = icon;
                    }
                }

                NavigationHelper.SetNavigateTo(topLevelItem, group.UniqueId);
                AutomationProperties.SetName(topLevelItem, group.Title);
                topLevelItem.InfoBadge = GetInfoBadge(group);

                var items = group.Items.Where(i => !i.HideNavigationViewItem);
                AddNavigationViewItemsRecursively(items, group.IsFooterNavigationViewItem, true, group.UniqueId);

                if (group.IsFooterNavigationViewItem)
                {
                    _navigationView.FooterMenuItems.Add(topLevelItem);
                }
                else
                {
                    _navigationView.MenuItems.Add(topLevelItem);
                }
            }
        }

        ConfigPages();
    }

    private InfoBadge GetInfoBadge(dynamic data)
    {
        if (data.DataInfoBadge is not null)
        {
            bool hideNavigationViewItemBadge = data.DataInfoBadge.HideNavigationViewItemBadge;
            string value = data.DataInfoBadge.BadgeValue;
            string style = data.DataInfoBadge.BadgeStyle;
            bool hasValue = !string.IsNullOrEmpty(value);
            if (style != null && style.Contains("Dot", StringComparison.OrdinalIgnoreCase) || style.Contains("Icon", StringComparison.OrdinalIgnoreCase))
            {
                hasValue = true;
            }
            if (!hideNavigationViewItemBadge && hasValue)
            {
                int badgeValue = Convert.ToInt32(data.DataInfoBadge.BadgeValue);
                int width = data.DataInfoBadge.BadgeWidth;
                int height = data.DataInfoBadge.BadgeHeight;

                InfoBadge infoBadge = new()
                {
                    Style = Application.Current.Resources[style] as Style
                };
                switch (style.ToLower())
                {
                    case string s when s.Contains("value"):
                        infoBadge.Value = badgeValue;
                        break;
                    case string s when s.Contains("icon"):
                        infoBadge.IconSource = GetIconSource(data.DataInfoBadge);
                        break;
                }

                if (width > 0 && height > 0)
                {
                    infoBadge.Width = width;
                    infoBadge.Height = height;
                }

                return infoBadge;
            }
        }
        return null;
    }

    private IconSource GetIconSource(DataInfoBadge infoBadge)
    {
        string symbol = infoBadge?.BadgeSymbolIcon;
        string image = infoBadge?.BadgeBitmapIcon;
        string glyph = infoBadge?.BadgeFontIconGlyph;
        string fontName = infoBadge?.BadgeFontIconFontName;

        if (!string.IsNullOrEmpty(symbol))
        {
            return new SymbolIconSource
            {
                Symbol = ApplicationHelper.GetEnum<Symbol>(symbol),
                Foreground = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as Brush,
            };
        }

        if (!string.IsNullOrEmpty(image))
        {
            return new BitmapIconSource
            {
                UriSource = new Uri(image),
                ShowAsMonochrome = false
            };
        }

        if (!string.IsNullOrEmpty(glyph))
        {
            var fontIcon = new FontIconSource
            {
                Glyph = ApplicationHelper.GetGlyph(glyph),
                Foreground = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as Brush,
            };
            if (!string.IsNullOrEmpty(fontName))
            {
                fontIcon.FontFamily = new FontFamily(fontName);
            }
            return fontIcon;
        }
        return null;
    }

    private IconElement GetIcon(string imagePath)
    {
        return string.IsNullOrEmpty(imagePath)
            ? null
            : imagePath.ToLowerInvariant().EndsWith(".png") || imagePath.ToLowerInvariant().EndsWith(".jpg")
                ? new BitmapIcon() { UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute), ShowAsMonochrome = false }
                : new FontIcon()
                {
                    Glyph = imagePath
                };
    }

    #region NavigationService
    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);
        if (pageType == null)
        {
            return false;
        }

        if (_frame != null && (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            Navigated?.Invoke(sender, e);
        }
    }

    #endregion
}
