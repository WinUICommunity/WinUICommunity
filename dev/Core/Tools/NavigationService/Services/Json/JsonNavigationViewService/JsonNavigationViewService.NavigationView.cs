﻿using Microsoft.UI.Xaml.Automation;

namespace WindowUI;
public partial class JsonNavigationViewService : IJsonNavigationViewService
{
    private void AddNavigationViewItemsRecursively(IEnumerable<DataItem> navItems, bool isFooterNavigationViewItem, bool hasTopLevel, string pageKey, NavigationViewItem parentNavItem = null)
    {
        if (_navigationView == null)
            return;

        foreach (var navItem in navItems)
        {
            var navigationViewItem = new NavigationViewItem()
            {
                IsEnabled = navItem.IncludedInBuild,
                Content = GetLocalizedText(navItem.Title, navItem.UsexUid),
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

            NavigationHelper.SetNavigateTo(navigationViewItem, navItem.UniqueId + navItem.Parameter?.ToString());
            navigationViewItem.InfoBadge = GetInfoBadge(navItem);
            AutomationProperties.SetName(navigationViewItem, GetLocalizedText(navItem.Title, navItem.UsexUid));

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

    private void AddNavigationMenuItems()
    {
        if (_navigationView == null)
            return;

        DataSource.GetDataAsync(JsonFilePath, _pathType, _autoIncludedInBuild).ContinueWith(t =>
        {
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
                        Content = GetLocalizedText(group.Title, group.UsexUid),
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
                    AutomationProperties.SetName(topLevelItem, GetLocalizedText(group.Title, group.UsexUid));
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
        }, TaskScheduler.FromCurrentSynchronizationContext());
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
                Symbol = GeneralHelper.GetEnum<Symbol>(symbol),
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
                Glyph = GeneralHelper.GetGlyph(glyph),
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

    private string GetLocalizedText(string input, bool usexUid)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        try
        {
            if (usexUid)
            {
                if (Localizer == null && ResourceManager != null && ResourceContext != null)
                {
                    var candidate = ResourceManager.MainResourceMap.TryGetValue($"Resources/{input}", ResourceContext);
                    return candidate != null ? candidate.ValueAsString : input;
                }
                else if (ResourceManager == null && Localizer != null)
                {
                    return Localizer.GetLocalizedString(input);
                }
            }
        }
        catch (Exception)
        {
            return input;
        }
        return input;
    }
}
