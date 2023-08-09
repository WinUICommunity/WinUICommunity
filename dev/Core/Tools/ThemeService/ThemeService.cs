using Nucs.JsonSettings;
using Nucs.JsonSettings.Fluent;
using Nucs.JsonSettings.Modulation;
using Nucs.JsonSettings.Modulation.Recovery;

namespace WinUICommunity;
public class ThemeService : IThemeService
{
    internal static CoreSettings Settings = JsonSettings.Configure<CoreSettings>()
                               .WithRecovery(RecoveryAction.RenameAndLoadDefault)
                               .WithVersioning(VersioningResultAction.RenameAndLoadDefault);

    public Window CurrentWindow { get; set; }
    public SystemBackdrop CurrentSystemBackdrop { get; set; }
    public Dictionary<Window, SystemBackdrop> CurrentSystemBackdropDic { get; set; } = new();
    public ElementTheme ActualTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    if (rootElement.RequestedTheme != ElementTheme.Default)
                    {
                        return rootElement.RequestedTheme;
                    }
                }
            }

            if (CurrentWindow != null && CurrentWindow.Content is FrameworkElement element)
            {
                if (element.RequestedTheme != ElementTheme.Default)
                {
                    return element.RequestedTheme;
                }
            }
            return ApplicationHelper.GetEnum<ElementTheme>(Application.Current.RequestedTheme.ToString());
        }
    }
    public ElementTheme RootTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    return rootElement.RequestedTheme;
                }
            }
            return CurrentWindow != null && CurrentWindow.Content is FrameworkElement element ? element.RequestedTheme : ElementTheme.Default;
        }
        set
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;
                }
            }

            if (CurrentWindow != null && CurrentWindow.Content is FrameworkElement element)
            {
                element.RequestedTheme = value;
            }
            if (this.useAutoSave)
            {
                Settings.ElementTheme = value;
                Settings?.Save();
            }
        }
    }
    public BackdropType CurrentBackdropType { get; set; }
    private bool useAutoSave;
    private string filename;
    private TitleBarCustomization titleBarCustomization;
    private Brush? backdropFallBackColorForWindows10;

    public event IThemeService.ActualThemeChangedEventHandler ActualThemeChanged;

    public void Initialize(Window window, bool useAutoSave = true, string filename = null)
    {
        CurrentWindow = window;

        foreach (Window activeWindow in WindowHelper.ActiveWindows)
        {
            if (activeWindow.Content is FrameworkElement rootElement)
            {
                WindowHelper.SetPreferredAppMode(rootElement.ActualTheme);
                rootElement.ActualThemeChanged += OnActualThemeChanged;
            }
        }

        if (CurrentWindow != null && CurrentWindow.Content is FrameworkElement element)
        {
            WindowHelper.SetPreferredAppMode(element.ActualTheme);
            element.ActualThemeChanged += OnActualThemeChanged;
        }

        string AppName = ApplicationHelper.GetProjectNameAndVersion();
        string RootPath = Path.Combine(ApplicationHelper.GetLocalFolderPath(), AppName);
        string AppConfigPath = Path.Combine(RootPath, "CommonAppConfig.json");

        this.useAutoSave = useAutoSave;
        this.filename = filename;

        if (!string.IsNullOrEmpty(filename))
        {
            AppConfigPath = filename;
        }

        Settings.LoadNow(AppConfigPath);
    }

    public void ConfigBackdrop(BackdropType backdropType = BackdropType.Mica, bool force = false)
    {
        if (useAutoSave && Settings.IsBackdropFirstRun)
        {
            Settings.BackdropType = backdropType;
            Settings.IsBackdropFirstRun = false;
            Settings.Save();
            CurrentBackdropType = backdropType;
        }

        if (backdropType != BackdropType.None)
        {
            foreach (Window _window in WindowHelper.ActiveWindows)
            {
                var systemBackdrop = GetCurrentSystemBackdropFromLocalConfig(backdropType, force);
                CurrentSystemBackdropDic.Add(_window, systemBackdrop);
                _window.SystemBackdrop = systemBackdrop;
                SetBackdropFallBackColorForWindows10(_window);
                CurrentBackdropType = GetBackdropType(systemBackdrop);
            }

            if (CurrentWindow != null)
            {
                var systemBackdrop = GetCurrentSystemBackdropFromLocalConfig(backdropType, force);
                CurrentSystemBackdrop = systemBackdrop;
                CurrentWindow.SystemBackdrop = systemBackdrop;
                SetBackdropFallBackColorForWindows10(CurrentWindow);

                CurrentBackdropType = GetBackdropType(systemBackdrop);
            }
        }
        else
        {
            CurrentBackdropType = BackdropType.None;
        }
    }

    public void ConfigElementTheme(ElementTheme elementTheme = ElementTheme.Default, bool force = false)
    {
        if (useAutoSave && Settings.IsThemeFirstRun)
        {
            Settings.ElementTheme = elementTheme;
            Settings.IsThemeFirstRun = false;
            Settings.Save();
        }

        if (useAutoSave)
        {
            SetCurrentTheme(GetCurrentThemeFromLocalConfig(elementTheme, force));
        }
        else
        {
            SetCurrentTheme(elementTheme);
        }
    }

    public void ConfigBackdropFallBackColorForWindow10(Brush brush)
    {
        backdropFallBackColorForWindows10 = brush;
    }

    public void ConfigTitleBar(TitleBarCustomization titleBarCustomization)
    {
        this.titleBarCustomization = titleBarCustomization;
        UpdateSystemCaptionButtonColors();
    }

    public BackdropType GetCurrentBackdropType()
    {
        return CurrentBackdropType;
    }

    public SystemBackdrop GetSystemBackdrop(BackdropType backdropType)
    {
        switch (backdropType)
        {
            case BackdropType.None:
                return null;
            case BackdropType.Mica:
                return new MicaBackdrop();
            case BackdropType.MicaAlt:
                return new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
            case BackdropType.DesktopAcrylic:
                return new DesktopAcrylicBackdrop();
            default:
                return null;
        }
    }

    public SystemBackdrop GetCurrentSystemBackdrop(Window activeWindow)
    {
        var currentWindow = CurrentSystemBackdropDic.FirstOrDefault(x => x.Key == activeWindow);
        return currentWindow.Value != null ? currentWindow.Key.SystemBackdrop : null;
    }

    public SystemBackdrop GetCurrentSystemBackdrop()
    {
        return CurrentSystemBackdrop != null ? CurrentWindow.SystemBackdrop : null;
    }

    public BackdropType GetBackdropType(SystemBackdrop systemBackdrop)
    {
        var backdropType = systemBackdrop?.GetType();
        if (backdropType == typeof(MicaBackdrop))
        {
            var mica = (MicaBackdrop)systemBackdrop;
            return mica.Kind == Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt ? BackdropType.MicaAlt : BackdropType.Mica;
        }
        else
        {
            return backdropType == typeof(DesktopAcrylicBackdrop) ? BackdropType.DesktopAcrylic : BackdropType.None;
        }
    }

    public void SetCurrentSystemBackdrop(BackdropType backdropType)
    {
        var systemBackdrop = GetSystemBackdrop(backdropType);
        CurrentBackdropType = backdropType;

        foreach (var key in CurrentSystemBackdropDic.Keys)
        {
            if (Settings.BackdropType != backdropType)
            {
                key.SystemBackdrop = systemBackdrop;
                CurrentSystemBackdropDic[key] = systemBackdrop;
            }

            SetBackdropFallBackColorForWindows10(key);
        }

        if (CurrentSystemBackdropDic != null)
        {
            if (Settings.BackdropType != backdropType)
            {
                CurrentWindow.SystemBackdrop = systemBackdrop;
            }

            SetBackdropFallBackColorForWindows10(CurrentWindow);
        }

        if (this.useAutoSave && Settings.BackdropType != backdropType)
        {
            Settings.BackdropType = backdropType;
            Settings?.Save();
        }
    }

    private SystemBackdrop GetCurrentSystemBackdropFromLocalConfig(BackdropType backdropType, bool ForceBackdrop)
    {
        BackdropType currentBackdrop = this.useAutoSave ? Settings.BackdropType : backdropType;
        return ForceBackdrop ? GetSystemBackdrop(backdropType) : GetSystemBackdrop(currentBackdrop);
    }

    private ElementTheme GetCurrentThemeFromLocalConfig(ElementTheme theme, bool forceTheme)
    {
        var currentTheme = Settings.ElementTheme;
        return forceTheme ? theme : currentTheme;
    }

    private void SetBackdropFallBackColorForWindows10(Window window)
    {
        if (OSVersionHelper.IsWindows10_1809_OrGreater && !OSVersionHelper.IsWindows11_22000_OrGreater && backdropFallBackColorForWindows10 != null)
        {
            var content = window.Content;
            if (content != null)
            {
                var element = window.Content as FrameworkElement;
                dynamic panel = (dynamic)element;
                panel.Background = backdropFallBackColorForWindows10;
            }
        }
    }

    private void OnActualThemeChanged(FrameworkElement sender, object args)
    {
        WindowHelper.SetPreferredAppMode(sender.ActualTheme);
        UpdateSystemCaptionButtonColors();
        ActualThemeChanged?.Invoke(sender, args);
    }

    public bool IsDarkTheme()
    {
        return RootTheme == ElementTheme.Default
            ? Application.Current.RequestedTheme == ApplicationTheme.Dark
            : RootTheme == ElementTheme.Dark;
    }

    public void UpdateSystemCaptionButtonForWindow(Window window)
    {
        var res = Application.Current.Resources;

        if (titleBarCustomization?.LightTitleBarButtons == null && titleBarCustomization?.DarkTitleBarButtons == null)
        {
            res["WindowCaptionBackground"] = Colors.Transparent;
            res["WindowCaptionBackgroundDisabled"] = Colors.Transparent;
        }
        else
        {
            if (IsDarkTheme())
            {
                res["WindowCaptionBackground"] = titleBarCustomization?.DarkTitleBarButtons?.ButtonBackgroundColor;
                res["WindowCaptionBackgroundDisabled"] = titleBarCustomization?.DarkTitleBarButtons?.ButtonInactiveBackgroundColor;
                res["WindowCaptionForeground"] = titleBarCustomization?.DarkTitleBarButtons?.ButtonForegroundColor;
                res["WindowCaptionForegroundDisabled"] = titleBarCustomization?.DarkTitleBarButtons?.ButtonInactiveForegroundColor;
            }
            else
            {
                res["WindowCaptionBackground"] = titleBarCustomization?.LightTitleBarButtons?.ButtonBackgroundColor;
                res["WindowCaptionBackgroundDisabled"] = titleBarCustomization?.LightTitleBarButtons?.ButtonInactiveBackgroundColor;
                res["WindowCaptionForeground"] = titleBarCustomization?.LightTitleBarButtons?.ButtonForegroundColor;
                res["WindowCaptionForegroundDisabled"] = titleBarCustomization?.LightTitleBarButtons?.ButtonInactiveForegroundColor;
            }
        }

        WindowHelper.ActivateWindowAgain(window);
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
        WindowHelper.ActivateWindowAgain(window);
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
                case TitleBarWindowType.Window:
                    UpdateSystemCaptionButtonForWindow(window);
                    break;
                case TitleBarWindowType.AppWindow:
                    UpdateSystemCaptionButtonForAppWindow(window);
                    break;
            }
        }
    }

    private void UpdateSystemCaptionButtonColors()
    {
        foreach (Window window in WindowHelper.ActiveWindows)
        {
            UpdateSystemCaptionButton(window);
        }

        if (CurrentWindow != null)
        {
            UpdateSystemCaptionButton(CurrentWindow);
        }
    }

    public void SetCurrentTheme(ElementTheme elementTheme)
    {
        if (RootTheme != elementTheme)
        {
            RootTheme = elementTheme;
        }
    }

    public ElementTheme GetCurrentTheme()
    {
        return RootTheme;
    }

    public void OnThemeComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedTheme = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = ApplicationHelper.GetEnum<ElementTheme>(selectedTheme);
            if (RootTheme != currentTheme)
            {
                RootTheme = currentTheme;
            }
        }
    }

    public void SetThemeComboBoxDefaultItem(ComboBox themeComboBox)
    {
        var currentTheme = RootTheme.ToString();
        var item = themeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme);
        if ((ComboBoxItem)themeComboBox.SelectedItem != item)
        {
            themeComboBox.SelectedItem = item;
        }
    }

    public void OnBackdropComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedBackdrop = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = ApplicationHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetCurrentSystemBackdrop(backdrop);
        }
    }

    public void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox)
    {
        var currentBackdrop = GetBackdropType(GetCurrentSystemBackdrop()).ToString();

        var item = backdropComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop);
        if ((ComboBoxItem)backdropComboBox.SelectedItem != item)
        {
            backdropComboBox.SelectedItem = item;
        }
    }

    public void OnThemeRadioButtonChecked(object sender)
    {
        var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = ApplicationHelper.GetEnum<ElementTheme>(selectedTheme);
            if (RootTheme != currentTheme)
            {
                RootTheme = currentTheme;
            }
        }
    }

    public void SetThemeRadioButtonDefaultItem(Panel ThemePanel)
    {
        var currentTheme = RootTheme.ToString();
        ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
    }

    public void OnBackdropRadioButtonChecked(object sender)
    {
        var selectedBackdrop = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = ApplicationHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetCurrentSystemBackdrop(backdrop);
        }
    }

    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetCurrentSystemBackdrop()).ToString();

        BackdropPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop).IsChecked = true;
    }
}
