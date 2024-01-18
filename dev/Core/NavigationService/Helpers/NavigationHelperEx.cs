namespace WinUICommunity;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public class NavigationHelperEx
{
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelperEx), new PropertyMetadata(null));

    public static NavigationViewItem GetParent(NavigationViewItem item) => (NavigationViewItem)item.GetValue(ParentProperty);

    public static void SetParent(NavigationViewItem item, NavigationViewItem value) => item.SetValue(ParentProperty, value);

    public static readonly DependencyProperty ParentProperty =
        DependencyProperty.RegisterAttached("Parent", typeof(NavigationViewItem), typeof(NavigationHelperEx), new PropertyMetadata(null));
}
