namespace WinUICommunity;

public partial class NavigationManager
{
    public NavigationManager() { }

    public static NavigationManager GetCurrent()
    {
        if (Instance == null)
        {
            _instance = new NavigationManager();
        }
        return Instance;
    }

    private async void InternalInitialize(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame, AutoSuggestBox autoSuggestBox, AutoSuggestBoxOptions autoSuggestBoxOptions)
    {
        this.navigationView = navigationView;
        this.frame = frame;
        this.autoSuggestBox = autoSuggestBox;

        if (navigationViewOptions != null)
        {
            this.DefaultPage = navigationViewOptions?.DefaultPage;
            this.SettingsPage = navigationViewOptions?.SettingsPage;

            if (navigationViewOptions.JsonOptions != null)
            {
                this.MenuFlyout = navigationViewOptions.JsonOptions.MenuFlyout;
                this.hasInfoBadge = navigationViewOptions.JsonOptions.HasInfoBadge;
                this.JsonFilePath = navigationViewOptions.JsonOptions.JsonFilePath;
                this.SectionPage = navigationViewOptions.JsonOptions.SectionPage;
                this.ItemPage = navigationViewOptions.JsonOptions.ItemPage;
                this.IncludedInBuildMode = navigationViewOptions.JsonOptions.IncludedInBuildMode;
                this.PathType = navigationViewOptions.JsonOptions.PathType;
                this.navigationViewEventsOptions = navigationViewOptions.EventsOptions;

                useJsonFile = true;
                ControlInfoDataSource = new ControlInfoDataSource();
                await AddNavigationMenuItems();
            }
        }

        if (this.navigationViewEventsOptions == EventsOptions.BuiltIn)
        {
            this.navigationView.ItemInvoked += OnItemInvoked;
            this.navigationView.BackRequested += OnBackRequested;
            this.navigationView.SelectionChanged += OnSelectionChanged;
        }

        if (autoSuggestBox != null)
        {
            if (autoSuggestBoxOptions != null)
            {
                this.autoSuggestBoxEventsOptions = autoSuggestBoxOptions.EventsOptions;
                this.NoResultString = autoSuggestBoxOptions.NoResultString;
                this.NoResultImage = autoSuggestBoxOptions.NoResultImage;
            }

            if (this.NoResultString == null)
            {
                this.NoResultString = "No result found";
            }

            if (this.autoSuggestBoxEventsOptions == EventsOptions.BuiltIn)
            {
                this.autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                this.autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
            }
        }

        this.Frame = frame;
        this.Navigated += Frame_Navigated;

        this.menuItems = GetAllMenuItems();

        if (this.DefaultPage != null)
        {
            this.Navigate(this.DefaultPage);
        }
    }

    public NavigationManager(NavigationView navigationView, Frame frame)
    {
        InternalInitialize(navigationView, null, frame, null, null);
    }
    public static NavigationManager Initialize(NavigationView navigationView, Frame frame)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, frame);
        }
        else
        {
            _instance.InternalInitialize(navigationView, null, frame, null, null);
        }
        return Instance;
    }

    public NavigationManager(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame)
    {
        InternalInitialize(navigationView, navigationViewOptions, frame, null, null);
    }
    public static NavigationManager Initialize(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, navigationViewOptions, frame);
        }
        else
        {
            _instance.InternalInitialize(navigationView, navigationViewOptions, frame, null, null);
        }
        return Instance;
    }

    public NavigationManager(NavigationView navigationView, Frame frame, AutoSuggestBox autoSuggestBox)
    {
        InternalInitialize(navigationView, null, frame, autoSuggestBox, null);
    }

    public static NavigationManager Initialize(NavigationView navigationView, Frame frame, AutoSuggestBox autoSuggestBox)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, frame, autoSuggestBox);
        }
        else
        {
            _instance.InternalInitialize(navigationView, null, frame, autoSuggestBox, null);
        }
        return Instance;
    }
    public NavigationManager(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame, AutoSuggestBox autoSuggestBox)
    {
        InternalInitialize(navigationView, navigationViewOptions, frame, autoSuggestBox, null);
    }

    public static NavigationManager Initialize(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame, AutoSuggestBox autoSuggestBox)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, navigationViewOptions, frame, autoSuggestBox);
        }
        else
        {
            _instance.InternalInitialize(navigationView, navigationViewOptions, frame, autoSuggestBox, null);
        }
        return Instance;
    }
    public NavigationManager(NavigationView navigationView, Frame frame, AutoSuggestBox autoSuggestBox, AutoSuggestBoxOptions autoSuggestBoxOptions)
    {
        InternalInitialize(navigationView, null, frame, autoSuggestBox, autoSuggestBoxOptions);
    }
    public static NavigationManager Initialize(NavigationView navigationView, Frame frame, AutoSuggestBox autoSuggestBox, AutoSuggestBoxOptions autoSuggestBoxOptions)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, frame, autoSuggestBox, autoSuggestBoxOptions);
        }
        else
        {
            _instance.InternalInitialize(navigationView, null, frame, autoSuggestBox, autoSuggestBoxOptions);
        }
        return Instance;
    }

    public NavigationManager(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame, AutoSuggestBox autoSuggestBox, AutoSuggestBoxOptions autoSuggestBoxOptions)
    {
        InternalInitialize(navigationView, navigationViewOptions, frame, autoSuggestBox, autoSuggestBoxOptions);
    }
    public static NavigationManager Initialize(NavigationView navigationView, NavigationViewOptions navigationViewOptions, Frame frame, AutoSuggestBox autoSuggestBox, AutoSuggestBoxOptions autoSuggestBoxOptions)
    {
        if (Instance == null)
        {
            _instance = new NavigationManager(navigationView, navigationViewOptions, frame, autoSuggestBox, autoSuggestBoxOptions);
        }
        else
        {
            _instance.InternalInitialize(navigationView, navigationViewOptions, frame, autoSuggestBox, autoSuggestBoxOptions);
        }
        return Instance;
    }

    public void LoadFromJson(string jsonFilePath, PathType pathType)
    {

    }
}
