using Nucs.JsonSettings;
using Nucs.JsonSettings.Fluent;
using Nucs.JsonSettings.Modulation;
using Nucs.JsonSettings.Modulation.Recovery;

namespace WinUICommunity;
public partial class ThemeService : IThemeService
{
    internal static CoreSettings Settings = JsonSettings.Configure<CoreSettings>()
                               .WithRecovery(RecoveryAction.RenameAndLoadDefault)
                               .WithVersioning(VersioningResultAction.RenameAndLoadDefault);

    public readonly string ConfigFilePath = "CommonAppConfig.json";
    public event IThemeService.ActualThemeChangedEventHandler ActualThemeChanged;
    private bool changeThemeWithoutSave = false;
    private bool useAutoSave;
    private string filename;
    public Window Window { get; set; }

    public ThemeService() { }
    public ThemeService(Window window)
    {
        Initialize(window);
        ConfigElementTheme();
        ConfigBackdrop();
    }
    public ElementTheme ActualTheme
    {
        get
        {
            if (Window != null && Window.Content is FrameworkElement element)
            {
                if (element.RequestedTheme != ElementTheme.Default)
                {
                    return element.RequestedTheme;
                }
            }
            return GeneralHelper.GetEnum<ElementTheme>(Application.Current.RequestedTheme.ToString());
        }
    }
    public void UpdateCaptionButtons()
    {
        UpdateCaptionButtons(Window);
    }

    public void UpdateCaptionButtons(Window window)
    {
        if (window == null)
            return;

        var res = Application.Current.Resources;
        Windows.UI.Color buttonForegroundColor;
        Windows.UI.Color buttonHoverForegroundColor;

        Windows.UI.Color buttonHoverBackgroundColor;
        if (ActualTheme == ElementTheme.Dark)
        {
            buttonForegroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#FFFFFF");
            buttonHoverForegroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#FFFFFF");

            buttonHoverBackgroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#0FFFFFFF");
        }
        else
        {
            buttonForegroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#191919");
            buttonHoverForegroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#191919");

            buttonHoverBackgroundColor = WinUICommunity.ColorHelper.GetColorFromHex("#09000000");
        }
        res["WindowCaptionForeground"] = buttonForegroundColor;

        window.AppWindow.TitleBar.ButtonForegroundColor = buttonForegroundColor;
        window.AppWindow.TitleBar.ButtonHoverForegroundColor = buttonHoverForegroundColor;

        window.AppWindow.TitleBar.ButtonHoverBackgroundColor = buttonHoverBackgroundColor;
    }
    public ElementTheme RootTheme
    {
        get
        {
            return Window != null && Window.Content is FrameworkElement element ? element.RequestedTheme : ElementTheme.Default;
        }
        set
        {
            if (Window != null && Window.Content is FrameworkElement element)
            {
                element.RequestedTheme = value;
            }
            if (!changeThemeWithoutSave)
            {
                if (this.useAutoSave)
                {
                    Settings.ElementTheme = value;
                    Settings?.Save();
                }
            }
        }
    }

    public void AutoInitialize(Window window)
    {
        Initialize(window);
        ConfigElementTheme();
        ConfigBackdrop();
    }

    public void Initialize(Window window, bool useAutoSave = true, string filename = null)
    {
        if (window == null)
        {
            return;
        }

        Window = window;

        if (Window.Content is FrameworkElement element)
        {
            GeneralHelper.SetPreferredAppMode(element.ActualTheme);
            element.ActualThemeChanged += OnActualThemeChanged;
        }

        var appInfo = AssemblyInfoHelper.GetAppDetails(NameType.CurrentAssemblyVersion, VersionType.AssemblyInformationalVersion);
        string AppName = appInfo.NameAndVersion;
        if (string.IsNullOrEmpty(appInfo.Version))
        {
            appInfo = AssemblyInfoHelper.GetAppDetails(NameType.CurrentAssemblyVersion, VersionType.CurrentAssemblyVersion);
            AppName = appInfo.NameAndVersion;
        }

        string RootPath = Path.Combine(PathHelper.GetApplicationDataFolderPath(), AppName);
        string AppConfigPath = Path.Combine(RootPath, ConfigFilePath);

        this.useAutoSave = useAutoSave;
        this.filename = filename;

        if (!string.IsNullOrEmpty(filename))
        {
            AppConfigPath = filename;
        }

        Settings.LoadNow(AppConfigPath);
    }

    private void OnActualThemeChanged(FrameworkElement sender, object args)
    {
        GeneralHelper.SetPreferredAppMode(sender.ActualTheme);
        ActualThemeChanged?.Invoke(sender, args);
    }
    
    public bool IsDarkTheme()
    {
        return RootTheme == ElementTheme.Default
            ? Application.Current.RequestedTheme == ApplicationTheme.Dark
            : RootTheme == ElementTheme.Dark;
    }
}
