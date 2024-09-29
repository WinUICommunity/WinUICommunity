namespace WinUICommunity;
public sealed partial class BreadcrumbNavigator : BreadcrumbBar
{
    public static readonly DependencyProperty PageTitle =
            DependencyProperty.RegisterAttached("PageTitle", typeof(string), typeof(BreadcrumbNavigator), new PropertyMetadata(null));

    public static void SetPageTitle(DependencyObject obj, string value)
    {
        obj.SetValue(PageTitle, value);
    }

    public static string GetPageTitle(DependencyObject obj)
    {
        return (string)obj.GetValue(PageTitle);
    }
}
