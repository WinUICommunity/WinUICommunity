﻿namespace WindowUI;

public interface INavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    void Initialize(NavigationView navigationView);
    void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, string notFoundString = null);

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);
}
