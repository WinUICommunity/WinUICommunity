using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity;

public static class NavigationExtension
{
    public static void EnsureNavigationSelection(this NavigationView navigationView, string id)
    {
        var isFound = EnsureNavigationSelection(navigationView, id, navigationView.MenuItems);
        if (!isFound)
        {
            EnsureNavigationSelection(navigationView, id, navigationView.FooterMenuItems);
        }
    }
    private static bool EnsureNavigationSelection(NavigationView navigationView, string id, IList<object> objects)
    {
        foreach (object rawGroup in objects)
        {
            if (rawGroup is NavigationViewItem group)
            {
                if (group.MenuItems == null || group.MenuItems.Count == 0)
                {
                    if ((string)group.Tag == id)
                    {
                        navigationView.SelectedItem = group;
                        group.IsSelected = true;
                        return true;
                    }
                }
                else
                {
                    bool found = EnsureNavigationSelection(navigationView, id, group.MenuItems);
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

    public static bool EnsureItemIsVisibleInNavigation(this NavigationView navigationView, string name)
    {
        bool changedSelection = false;
        changedSelection = EnsureItemIsVisibleInNavigation(navigationView, name, navigationView.MenuItems);
        if (!changedSelection)
        {
            changedSelection = EnsureItemIsVisibleInNavigation(navigationView, name, navigationView.FooterMenuItems);
        }
        return changedSelection;
    }
    private static bool EnsureItemIsVisibleInNavigation(NavigationView navigationView, string name, IList<object> objects)
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
                    bool found = EnsureItemIsVisibleInNavigation(navigationView, name, item.MenuItems);
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
}
