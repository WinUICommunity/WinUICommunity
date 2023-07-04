using Nucs.JsonSettings;
using Nucs.JsonSettings.Fluent;
using Nucs.JsonSettings.Modulation;
using Nucs.JsonSettings.Modulation.Recovery;

namespace WinUICommunity;

public partial class ThemeManager
{
    internal static CoreSettings Settings = JsonSettings.Configure<CoreSettings>()
                               .WithRecovery(RecoveryAction.RenameAndLoadDefault)
                               .WithVersioning(VersioningResultAction.RenameAndLoadDefault);
    public ThemeManager() { }

    public static ThemeManager GetCurrent()
    {
        if (Instance == null)
        {
            instance = new ThemeManager();
        }
        return Instance;
    }

    private void InternalInitialize(Window window, ThemeOptions themeOptions)
    {
        currentWindow = window;
        foreach (Window activeWindow in WindowHelper.ActiveWindows)
        {
            if (activeWindow.Content is FrameworkElement rootElement)
            {
                WindowHelper.SetPreferredAppMode(rootElement.ActualTheme);
                rootElement.ActualThemeChanged += OnActualThemeChanged;
            }
        }

        if (currentWindow != null && currentWindow.Content is FrameworkElement element)
        {
            WindowHelper.SetPreferredAppMode(element.ActualTheme);
            element.ActualThemeChanged += OnActualThemeChanged;
        }

        string AppName = ApplicationHelper.GetProjectNameAndVersion();
        string RootPath = Path.Combine(ApplicationHelper.GetLocalFolderPath(), AppName);
        string AppConfigPath = Path.Combine(RootPath, "CommonAppConfig.json");

        if (themeOptions != null)
        {
            this.ThemeOptions = themeOptions;
            this.useBuiltInSettings = themeOptions.UseBuiltInSettings;

            if (!string.IsNullOrEmpty(themeOptions.BuiltInSettingsFileName))
            {
                AppConfigPath = themeOptions.BuiltInSettingsFileName;
            }

            if (this.useBuiltInSettings)
            {
                Settings.LoadNow(AppConfigPath);
                if (Settings.IsFirstRun)
                {
                    Settings.ElementTheme = themeOptions.ElementTheme;
                    Settings.BackdropType = themeOptions.BackdropType;
                    Settings.IsFirstRun = false;
                    Settings?.Save();
                }
            }

            if (themeOptions.BackdropType != BackdropType.None)
            {
                foreach (Window _window in WindowHelper.ActiveWindows)
                {
                    var systemBackdrop = GetCurrentSystemBackdropFromLocalConfig(themeOptions.BackdropType, themeOptions.ForceBackdrop);
                    currentSystemBackdropDic.Add(_window, systemBackdrop);
                    _window.SystemBackdrop = systemBackdrop;
                    SetBackdropFallBackColorForWindows10(_window);
                }

                if (currentWindow != null)
                {
                    var systemBackdrop = GetCurrentSystemBackdropFromLocalConfig(themeOptions.BackdropType, themeOptions.ForceBackdrop);
                    currentSystemBackdrop = systemBackdrop;
                    currentWindow.SystemBackdrop = systemBackdrop;
                    SetBackdropFallBackColorForWindows10(currentWindow);
                }
            }

            if (this.useBuiltInSettings)
            {
                SetCurrentTheme(GetCurrentThemeFromLocalConfig(themeOptions.ElementTheme, themeOptions.ForceElementTheme));
            }
            else
            {
                SetCurrentTheme(themeOptions.ElementTheme);
            }
        }
        else
        {
            Settings.LoadNow(AppConfigPath);
            SetCurrentTheme(Settings.ElementTheme);
        }

        UpdateSystemCaptionButtonColors();
    }

    public static ThemeManager Initialize()
    {
        if (Instance == null)
        {
            instance = new ThemeManager(null, null);
        }
        else
        {
            instance.InternalInitialize(null, null);
        }
        return Instance;
    }
    public static ThemeManager Initialize(Window window)
    {
        if (Instance == null)
        {
            instance = new ThemeManager(window, null);
        }
        else
        {
            instance.InternalInitialize(window, null);
        }
        return Instance;
    }
    public static ThemeManager Initialize(ThemeOptions themeOptions)
    {
        if (Instance == null)
        {
            instance = new ThemeManager(null, themeOptions);
        }
        else
        {
            instance.InternalInitialize(null, themeOptions);
        }
        return Instance;
    }
    public static ThemeManager Initialize(Window window, ThemeOptions themeOptions)
    {
        if (Instance == null)
        {
            instance = new ThemeManager(window, themeOptions);
        }
        else
        {
            instance.InternalInitialize(window, themeOptions);
        }
        return Instance;
    }

    public ThemeManager(Window window)
    {
        InternalInitialize(window, null);
    }

    public ThemeManager(ThemeOptions themeOptions)
    {
        InternalInitialize(null, themeOptions);
    }

    public ThemeManager(Window window, ThemeOptions themeOptions)
    {
        InternalInitialize(window, themeOptions);
    }
}
