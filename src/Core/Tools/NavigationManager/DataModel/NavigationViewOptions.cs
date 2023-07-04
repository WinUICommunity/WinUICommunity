namespace WinUICommunity;

public class NavigationViewOptions
{
    public Type DefaultPage { get; set; }
    public Type SettingsPage { get; set; }
    public JsonOptions JsonOptions { get; set; }
    public EventsOptions EventsOptions { get; set; } = EventsOptions.BuiltIn;
}
