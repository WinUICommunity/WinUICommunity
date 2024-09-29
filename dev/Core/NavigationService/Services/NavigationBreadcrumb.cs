namespace WinUICommunity;
//thanks to https://github.com/microsoft/devhome/blob/main/settings/DevHome.Settings/Models/Breadcrumb.cs#L10
public class NavigationBreadcrumb
{
    public NavigationBreadcrumb(string label, Type page)
    {
        Label = label;
        Page = page;
    }
    public string Label { get; }

    public Type Page { get; }

    public override string ToString() => Label;
}
