using Microsoft.UI.Xaml.Automation;

namespace WinUICommunity;

public partial class NavigationManager : Observable
{
    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        GoBack();
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        navigationView.IsBackEnabled = CanGoBack;
        if (e.SourcePageType == SettingsPage)
        {
            navigationView.SelectedItem = (NavigationViewItem)navigationView.SettingsItem;
        }
        else if (e.SourcePageType != null && !useJsonFile)
        {
            SetSelectedMenuItem(navigationView.MenuItems, e.SourcePageType);
        }
        else if (e.SourcePageType != null && useJsonFile)
        {
            if (e.Parameter == null || (e.Parameter != null && (((NavigationArgs)e.Parameter).Parameter == null)))
            {
                EnsureNavigationSelection(e.Content.ToString());
            }
            else
            {
                EnsureNavigationSelection(((NavigationArgs)e.Parameter).Parameter?.ToString());
            }
        }
    }

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (!args.IsSettingsSelected && useJsonFile)
        {
            if (this.SectionPage != null && this.ItemPage != null)
            {
                var selectedItem = args.SelectedItemContainer;
                if (selectedItem.DataContext is ControlInfoDataGroup itemGroup && !string.IsNullOrEmpty(itemGroup.UniqueId))
                {
                    NavigateForJson(this.SectionPage, itemGroup.UniqueId);
                }
                else if (selectedItem.DataContext is ControlInfoDataItem item && !string.IsNullOrEmpty(item.UniqueId))
                {
                    NavigateForJson(this.ItemPage, item.UniqueId);
                }
            }
            else if (this.SectionPage != null && this.ItemPage == null)
            {
                var selectedItem = args.SelectedItemContainer;
                if (selectedItem.DataContext is ControlInfoDataGroup itemGroup && !string.IsNullOrEmpty(itemGroup.UniqueId))
                {
                    NavigateForJson(this.SectionPage, itemGroup.UniqueId);
                }
                else if (selectedItem.DataContext is ControlInfoDataItem item)
                {
                    NavigateWithUniqueId(item);
                }
            }
            else if (this.ItemPage != null && this.SectionPage == null)
            {
                var selectedItem = args.SelectedItemContainer;
                if (selectedItem.DataContext is ControlInfoDataGroup itemGroup)
                {
                    NavigateWithUniqueId(itemGroup);
                }
                else if (selectedItem.DataContext is ControlInfoDataItem item && !string.IsNullOrEmpty(item.UniqueId))
                {
                    NavigateForJson(this.ItemPage, item.UniqueId);
                }
            }
            else if (this.SectionPage == null && this.ItemPage == null)
            {
                var selectedItem = args.SelectedItemContainer;
                NavigateWithUniqueId(selectedItem.DataContext);
            }
        }
    }

    private void NavigateWithUniqueId(dynamic item)
    {
        if (string.IsNullOrEmpty(item.UniqueId))
        {
            return;
        }
        Assembly assembly = string.IsNullOrEmpty(item.ApiNamespace) ? Application.Current.GetType().Assembly : (Assembly)Assembly.Load(item.ApiNamespace);
        if (assembly is not null)
        {

            Type pageType = assembly.GetType(item.UniqueId);
            NavigateForJson(pageType);
        }
    }

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true && SettingsPage != null)
        {
            Navigate(SettingsPage);
        }
        else if (args.InvokedItemContainer != null && !useJsonFile)
        {
            var item = menuItems.FirstOrDefault(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            if (item != null)
            {
                var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
                var itemTag = item.Tag?.ToString();
                Navigate(pageType);
            }
        }
    }

    private void SetSelectedMenuItem(IList<object> items, Type pageType)
    {
        foreach (NavigationViewItemBase item in items)
        {
            var itemType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            if (itemType != null && itemType.FullName == pageType.FullName)
            {
                navigationView.SelectedItem = item;
                return;
            }

            // If the item has nested items, recursively search them
            if (item is NavigationViewItem && ((NavigationViewItem)item).MenuItems.Any())
            {
                SetSelectedMenuItem(((NavigationViewItem)item).MenuItems, pageType);
            }
        }

        // If no item has been selected yet, search the footer items
        foreach (var footerObject in navigationView.FooterMenuItems)
        {
            if (footerObject is NavigationViewItem footerItem)
            {
                var itemType = footerItem.GetValue(NavHelper.NavigateToProperty) as Type;
                if (itemType != null && itemType.FullName == pageType.FullName)
                {
                    navigationView.SelectedItem = footerItem;
                    return;
                }

                // If the footer item has nested items, recursively search them
                if (footerItem is NavigationViewItem && ((NavigationViewItem)footerItem).MenuItems.Any())
                {
                    SetSelectedMenuItem(((NavigationViewItem)footerItem).MenuItems, pageType);
                }
            }
        }
    }

    public IEnumerable<NavigationViewItem> GetAllMenuItems()
    {
        var _menuItems = EnumerateNavigationViewItem(navigationView.MenuItems);
        var footer = EnumerateNavigationViewItem(navigationView.FooterMenuItems);
        return _menuItems.Concat(footer);
    }

    private IEnumerable<NavigationViewItem> EnumerateNavigationViewItem(IList<object> parent)
    {
        if (parent != null)
        {
            foreach (var g in parent)
            {
                yield return (NavigationViewItem)g;

                foreach (var sub in EnumerateNavigationViewItem(((NavigationViewItem)g).MenuItems))
                {
                    yield return sub;
                }
            }
        }
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            if (useJsonFile && ItemPage == null && args.ChosenSuggestion is ControlInfoDataItem)
            {
                var infoDataItem = args.ChosenSuggestion as ControlInfoDataItem;
                var hasChangedSelection = EnsureItemIsVisibleInNavigation(infoDataItem.Title);

                // In case the menu selection has changed, it means that it has triggered
                // the selection changed event, that will navigate to the page already
                if (!hasChangedSelection)
                {
                    NavigateWithUniqueId(infoDataItem);
                }
            }
            else if (useJsonFile && ItemPage != null && args.ChosenSuggestion is ControlInfoDataItem)
            {
                var infoDataItem = args.ChosenSuggestion as ControlInfoDataItem;
                var hasChangedSelection = EnsureItemIsVisibleInNavigation(infoDataItem.Title);

                // In case the menu selection has changed, it means that it has triggered
                // the selection changed event, that will navigate to the page already
                if (!hasChangedSelection)
                {
                    NavigateForJson(ItemPage, infoDataItem.UniqueId);
                }
            }
            else
            {
                var item = args.ChosenSuggestion as string;
                navigationView.SelectedItem = navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(menuItem => (string)menuItem.Content == item);

                var pageType = ((NavigationViewItem)(navigationView.SelectedItem)).GetValue(NavHelper.NavigateToProperty) as Type;
                Navigate(pageType);
            }
        }
    }

    private IEnumerable<ControlInfoDataItem> SearchNavigationViewItems(IEnumerable<ControlInfoDataItem> items, string query)
    {
        query = query.Trim()?.ToLowerInvariant(); // Convert query to lowercase for case-insensitive comparison

        foreach (var item in items)
        {
            if (item.Title.ToLowerInvariant().Contains(query)) // Use ToLowerInvariant for case-insensitive comparison
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

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (useJsonFile)
            {
                var matches = SearchNavigationViewItems(ControlInfoDataSource.Groups.SelectMany(group => group.Items), sender.Text);

                if (matches.Any())
                {
                    autoSuggestBox.ItemsSource = matches.OrderByDescending(i => i.Title.StartsWith(sender.Text.ToLowerInvariant())).ThenBy(i => i.Title);
                }
                else
                {
                    var noResultsItem = new ControlInfoDataItem(NoResultString, NoResultImage);
                    var noResultsList = new List<ControlInfoDataItem>();
                    noResultsList.Add(noResultsItem);
                    autoSuggestBox.ItemsSource = noResultsList;
                }
            }
            else
            {
                var suggestions = new List<string>();
                var history = navigationView.MenuItems.OfType<NavigationViewItem>().ToList();

                var querySplit = autoSuggestBox.Text.Split(' ');
                var matchingItems = history.Where(
                    item =>
                    {
                        var flag = true;
                        foreach (var queryToken in querySplit)
                        {
                            if (item.Content.ToString().IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                            {
                                flag = false;
                            }

                        }
                        return flag;
                    });

                foreach (var item in matchingItems)
                {
                    suggestions.Add(item.Content.ToString());
                }
                autoSuggestBox.ItemsSource = suggestions.Count > 0 ? suggestions : (new string[] { NoResultString });
            }

        }
    }

    #region Work with Json File
    private NavigationViewItem topLevelItem { get; set; }
    private void AddNavigationViewItemsRecursively(IEnumerable<ControlInfoDataItem> navItems, bool isFooterNavigationViewItem, bool hasTopLevel, NavigationViewItem parentNavItem = null)
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

            navigationViewItem.InfoBadge = GetInfoBadge(navItem);
            AutomationProperties.SetName(navigationViewItem, navItem.Title);

            if (this.MenuFlyout != null)
            {
                navigationViewItem.ContextFlyout = this.MenuFlyout;
            }

            if (parentNavItem == null)
            {
                if (hasTopLevel)
                {
                    topLevelItem.MenuItems.Add(navigationViewItem);
                }
                else
                {
                    if (isFooterNavigationViewItem)
                    {
                        navigationView.FooterMenuItems.Add(navigationViewItem);
                    }
                    else
                    {
                        navigationView.MenuItems.Add(navigationViewItem);
                    }
                }
            }
            else
            {
                parentNavItem.MenuItems.Add(navigationViewItem);
            }

            if (navItem.Items != null && navItem.Items.Count > 0)
            {
                AddNavigationViewItemsRecursively(navItem.Items, isFooterNavigationViewItem, hasTopLevel, navigationViewItem);
            }
        }
    }

    private async Task AddNavigationMenuItems()
    {
        await ControlInfoDataSource.GetControlInfoDataAsync(this.JsonFilePath, this.PathType, this.IncludedInBuildMode);

        foreach (var group in ControlInfoDataSource.Groups.OrderBy(i => i.Title).Where(i => !i.IsSpecialSection && !i.HideGroup))
        {
            if (group.ShowItemsWithoutGroup)
            {
                var items = group.Items.Where(i => !i.HideNavigationViewItem);
                AddNavigationViewItemsRecursively(items, group.IsFooterNavigationViewItem, false);
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

                AutomationProperties.SetName(topLevelItem, group.Title);
                topLevelItem.InfoBadge = GetInfoBadge(group);

                if (this.MenuFlyout != null)
                {
                    topLevelItem.ContextFlyout = this.MenuFlyout;
                }

                var items = group.Items.Where(i => !i.HideNavigationViewItem);
                AddNavigationViewItemsRecursively(items, group.IsFooterNavigationViewItem, true);

                if (group.IsFooterNavigationViewItem)
                {
                    navigationView.FooterMenuItems.Add(topLevelItem);
                }
                else
                {
                    navigationView.MenuItems.Add(topLevelItem);
                }
            }
        }
    }

    private InfoBadge GetInfoBadge(dynamic controlInfoData)
    {
        if (controlInfoData.InfoBadge is not null)
        {
            bool hideNavigationViewItemBadge = controlInfoData.InfoBadge.HideNavigationViewItemBadge;
            string value = controlInfoData.InfoBadge.BadgeValue;
            string style = controlInfoData.InfoBadge.BadgeStyle;
            bool hasValue = !string.IsNullOrEmpty(value);
            if (style != null && style.Contains("Dot", StringComparison.OrdinalIgnoreCase) || style.Contains("Icon", StringComparison.OrdinalIgnoreCase))
            {
                hasValue = true;
            }
            if (hasInfoBadge && !hideNavigationViewItemBadge && hasValue)
            {
                int badgeValue = Convert.ToInt32(controlInfoData.InfoBadge.BadgeValue);
                int width = controlInfoData.InfoBadge.BadgeWidth;
                int height = controlInfoData.InfoBadge.BadgeHeight;

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
                        infoBadge.IconSource = GetIconSource(controlInfoData.InfoBadge);
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

    private IconSource GetIconSource(ControlInfoBadge infoBadge)
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

    public IconElement GetIcon(string imagePath)
    {
        return string.IsNullOrEmpty(imagePath)
            ? null
            : imagePath.ToLowerInvariant().EndsWith(".png") ?
                    new BitmapIcon() { UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute), ShowAsMonochrome = false } :
                    new FontIcon()
                    {
                        Glyph = imagePath
                    };
    }

    public void EnsureNavigationSelection(string id)
    {
        var isFound = EnsureNavigationSelection(id, this.navigationView.MenuItems);
        if (!isFound)
        {
            EnsureNavigationSelection(id, this.navigationView.FooterMenuItems);
        }
    }

    private bool EnsureNavigationSelection(string id, IList<object> objects)
    {
        foreach (object rawGroup in objects)
        {
            if (rawGroup is NavigationViewItem group)
            {
                if (group.MenuItems == null || group.MenuItems.Count == 0)
                {
                    if ((string)group.Tag == id)
                    {
                        this.navigationView.SelectedItem = group;
                        group.IsSelected = true;
                        return true;
                    }
                }
                else
                {
                    bool found = EnsureNavigationSelection(id, group.MenuItems);
                    if (found)
                    {
                        group.IsExpanded = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool EnsureItemIsVisibleInNavigation(string name)
    {
        bool changedSelection = false;
        changedSelection = EnsureItemIsVisibleInNavigation(name, this.navigationView.MenuItems);
        if (!changedSelection)
        {
            changedSelection = EnsureItemIsVisibleInNavigation(name, this.navigationView.FooterMenuItems);
        }
        return changedSelection;
    }

    private bool EnsureItemIsVisibleInNavigation(string name, IList<object> objects)
    {
        bool changedSelection = false;
        foreach (var rawItem in objects)
        {
            if (rawItem is NavigationViewItem item)
            {
                if (item.MenuItems == null || item.MenuItems.Count == 0)
                {
                    if ((string)item.Tag == name)
                    {
                        navigationView.SelectedItem = item;
                        item.StartBringIntoView();
                        changedSelection = true;
                        break;
                    }
                }
                else
                {
                    bool found = EnsureItemIsVisibleInNavigation(name, item.MenuItems);
                    if (found)
                    {
                        item.IsExpanded = true;
                        navigationView.UpdateLayout();
                        changedSelection = true;
                        break;
                    }
                }
            }
        }

        return changedSelection;
    }
    #endregion
}
