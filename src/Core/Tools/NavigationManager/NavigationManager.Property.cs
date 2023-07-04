namespace WinUICommunity;

public partial class NavigationManager
{
    private static NavigationManager _instance;
    public static NavigationManager Instance => _instance;

    private EventsOptions navigationViewEventsOptions { get; set; }
    private EventsOptions autoSuggestBoxEventsOptions { get; set; }
    public NavigationView navigationView { get; set; }
    public AutoSuggestBox autoSuggestBox { get; set; }
    private IEnumerable<NavigationViewItem> menuItems { get; set; }
    public IncludedInBuildMode IncludedInBuildMode { get; set; }
    public ControlInfoDataSource ControlInfoDataSource { get; set; }
    public MenuFlyout MenuFlyout { get; set; }
    public string JsonFilePath { get; set; }
    public PathType PathType { get; set; }
    private bool useJsonFile { get; set; }
    private bool hasInfoBadge { get; set; }

    public event NavigatedEventHandler Navigated;
    public event NavigationFailedEventHandler NavigationFailed;

    private object lastParamUsed;
    private Frame frame;
    public Frame Frame
    {
        get
        {
            if (frame == null)
            {
                frame = Window.Current.Content as Frame;
                RegisterFrameEvents();
            }

            return frame;
        }

        set
        {
            UnregisterFrameEvents();
            Set(ref frame, value);
            RegisterFrameEvents();
        }
    }

    public bool CanGoBack => Frame.CanGoBack;

    public bool CanGoForward => Frame.CanGoForward;

    public bool GoBack()
    {
        if (CanGoBack)
        {
            Frame.GoBack();
            return true;
        }

        return false;
    }

    private Type _settingsPage;
    public Type SettingsPage
    {
        get => _settingsPage;
        set => Set(ref _settingsPage, value);
    }

    private Type _sectionPage;
    public Type SectionPage
    {
        get => _sectionPage;
        set => Set(ref _sectionPage, value);
    }

    private Type _itemPage;
    public Type ItemPage
    {
        get => _itemPage;
        set => Set(ref _itemPage, value);
    }

    private Type _defaultPage;
    public Type DefaultPage
    {
        get => _defaultPage;
        set => Set(ref _defaultPage, value);
    }

    private string _noResultString;
    public string NoResultString
    {
        get => _noResultString;
        set => Set(ref _noResultString, value);
    }
    private string _noResultImage;
    public string NoResultImage
    {
        get => _noResultImage;
        set => Set(ref _noResultImage, value);
    }
}
