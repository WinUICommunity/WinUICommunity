using Microsoft.UI.Composition.SystemBackdrops;
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
    public BackdropType CurrentBackdropType { get; set; }
    private TitleBarCustomization titleBarCustomization;
    private Brush? backdropFallBackColorForWindows10;
    public Window Window { get; set; }
    public SystemBackdrop CurrentSystemBackdrop { get; set; }

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

        var appInfo = AssemblyInfoHelper.GetAppInfo(NameType.CurrentAssemblyVersion, VersionType.AssemblyInformationalVersion);
        string AppName = appInfo.NameAndVersion;
        if (string.IsNullOrEmpty(appInfo.Version))
        {
            appInfo = AssemblyInfoHelper.GetAppInfo(NameType.CurrentAssemblyVersion, VersionType.CurrentAssemblyVersion);
            AppName = appInfo.NameAndVersion;
        }

        string RootPath = Path.Combine(PathHelper.GetLocalFolderPath(), AppName);
        string AppConfigPath = Path.Combine(RootPath, ConfigFilePath);

        this.useAutoSave = useAutoSave;
        this.filename = filename;

        if (!string.IsNullOrEmpty(filename))
        {
            AppConfigPath = filename;
        }

        Settings.LoadNow(AppConfigPath);
    }

    private ISystemBackdropController ResetBackdrop()
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop is MicaBackdrop)
        {
            return new MicaBackdrop().micaController;
        }
        else if (backdrop is AcrylicBackdrop)
        {
            return new AcrylicBackdrop().acrylicController;
        }
        return null;
    }

    private void OnActualThemeChanged(FrameworkElement sender, object args)
    {
        GeneralHelper.SetPreferredAppMode(sender.ActualTheme);
        UpdateSystemCaptionButtonColors();
        ActualThemeChanged?.Invoke(sender, args);
    }

    public bool IsDarkTheme()
    {
        return RootTheme == ElementTheme.Default
            ? Application.Current.RequestedTheme == ApplicationTheme.Dark
            : RootTheme == ElementTheme.Dark;
    }

    public void UpdateSystemCaptionButtonForAppWindow(Window window)
    {
        var titleBar = window.AppWindow.TitleBar;
        if (titleBarCustomization?.LightTitleBarButtons == null && titleBarCustomization?.DarkTitleBarButtons == null)
        {
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            if (IsDarkTheme())
            {
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            }
            else
            {
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            }
        }
        else
        {
            if (IsDarkTheme())
            {
                titleBar.BackgroundColor = titleBarCustomization?.DarkTitleBarButtons?.BackgroundColor;
                titleBar.ForegroundColor = titleBarCustomization?.DarkTitleBarButtons?.ForegroundColor;
                titleBar.ButtonForegroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonForegroundColor;
                titleBar.ButtonBackgroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonBackgroundColor;
                titleBar.ButtonHoverBackgroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonHoverBackgroundColor;
                titleBar.ButtonHoverForegroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonHoverForegroundColor;
                titleBar.ButtonInactiveBackgroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonInactiveBackgroundColor;
                titleBar.ButtonInactiveForegroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonInactiveForegroundColor;
                titleBar.ButtonPressedBackgroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonPressedBackgroundColor;
                titleBar.ButtonPressedForegroundColor = titleBarCustomization?.DarkTitleBarButtons?.ButtonPressedForegroundColor;
            }
            else
            {
                titleBar.BackgroundColor = titleBarCustomization?.LightTitleBarButtons?.BackgroundColor;
                titleBar.ForegroundColor = titleBarCustomization?.LightTitleBarButtons?.ForegroundColor;
                titleBar.ButtonForegroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonForegroundColor;
                titleBar.ButtonBackgroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonBackgroundColor;
                titleBar.ButtonHoverBackgroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonHoverBackgroundColor;
                titleBar.ButtonHoverForegroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonHoverForegroundColor;
                titleBar.ButtonInactiveBackgroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonInactiveBackgroundColor;
                titleBar.ButtonInactiveForegroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonInactiveForegroundColor;
                titleBar.ButtonPressedBackgroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonPressedBackgroundColor;
                titleBar.ButtonPressedForegroundColor = titleBarCustomization?.LightTitleBarButtons?.ButtonPressedForegroundColor;
            }
        }
    }

    public void ResetCaptionButtonColors(Window window)
    {
        var res = Application.Current.Resources;

        window.AppWindow.TitleBar.BackgroundColor = null;
        window.AppWindow.TitleBar.ButtonBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonInactiveBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonHoverBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonPressedBackgroundColor = null;
        window.AppWindow.TitleBar.ForegroundColor = null;
        window.AppWindow.TitleBar.ButtonForegroundColor = null;
        window.AppWindow.TitleBar.ButtonInactiveForegroundColor = null;
        window.AppWindow.TitleBar.ButtonHoverForegroundColor = null;
        window.AppWindow.TitleBar.ButtonPressedForegroundColor = null;
        res["WindowCaptionBackground"] = res["SystemControlBackgroundBaseLowBrush"];
        res["WindowCaptionBackgroundDisabled"] = res["SystemControlBackgroundBaseLowBrush"];
        res["WindowCaptionForeground"] = res["SystemControlForegroundBaseHighBrush"];
        res["WindowCaptionForegroundDisabled"] = res["SystemControlDisabledBaseMediumLowBrush"];
        WindowHelper.ReActivateWindow(window);
    }

    public void UpdateSystemCaptionButton(Window window)
    {
        if (this.titleBarCustomization != null)
        {
            switch (this.titleBarCustomization.TitleBarWindowType)
            {
                case TitleBarWindowType.None:
                    ResetCaptionButtonColors(window);
                    break;
                case TitleBarWindowType.AppWindow:
                    UpdateSystemCaptionButtonForAppWindow(window);
                    break;
            }
        }
    }

    private void UpdateSystemCaptionButtonColors()
    {
        if (Window != null)
        {
            UpdateSystemCaptionButton(Window);
        }
    }

    public void ResetBackdropProperties()
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaBackdrop mica)
            {
                mica.micaController.ResetProperties();
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.acrylicController.ResetProperties();
            }

            Settings.BackdropFallBackColor = GetDefaultBackdropFallBackColor();
            Settings.BackdropTintColor = GetDefaultBackdropTintColor();
            Settings.BackdropTintOpacity = GetDefaultBackdropTintOpacity();
            Settings.BackdropLuminosityOpacity = GetDefaultBackdropLuminosityOpacity();
            Settings.Save();
        }
    }
}
