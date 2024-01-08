using Microsoft.UI.Composition.SystemBackdrops;
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
    public ElementTheme ActualTheme
    {
        get
        {
            if (CurrentWindow != null && CurrentWindow.Content is FrameworkElement element)
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
            return CurrentWindow != null && CurrentWindow.Content is FrameworkElement element ? element.RequestedTheme : ElementTheme.Default;
        }
        set
        {
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
        if (window == null)
        {
            return;
        }
        CurrentWindow = window;

        if (CurrentWindow.Content is FrameworkElement element)
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
        if (CurrentWindow == null)
        {
            return;
        }

        if (useAutoSave && Settings.IsBackdropFirstRun)
        {
            Settings.BackdropType = backdropType;
            Settings.IsBackdropFirstRun = false;
            Settings.Save();
            CurrentBackdropType = backdropType;
        }

        if (backdropType != BackdropType.None)
        {
            var systemBackdrop = GetCurrentSystemBackdropFromLocalConfig(backdropType, force);
            CurrentSystemBackdrop = systemBackdrop;
            SetWindowBackdrop(systemBackdrop);
            SetBackdropFallBackColorForWindows10(CurrentWindow);

            CurrentBackdropType = GetBackdropType(systemBackdrop);
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
                return new MicaBackdrop { Kind = MicaKind.BaseAlt };
            case BackdropType.DesktopAcrylic:
                return new DesktopAcrylicBackdrop();
            case BackdropType.AcrylicBase:
                return new AcrylicBackdrop() { Kind = DesktopAcrylicKind.Base };
            case BackdropType.AcrylicThin:
                return new AcrylicBackdrop() { Kind = DesktopAcrylicKind.Thin };
            case BackdropType.Transparent:
                return new TransparentBackdrop();
            default:
                return null;
        }
    }

    public SystemBackdrop GetCurrentSystemBackdrop()
    {
        return CurrentSystemBackdrop;
    }

    public BackdropType GetBackdropType(SystemBackdrop systemBackdrop)
    {
        var backdropType = systemBackdrop?.GetType();
        if (backdropType == typeof(MicaBackdrop))
        {
            var mica = (MicaBackdrop)systemBackdrop;
            return mica.Kind == MicaKind.BaseAlt ? BackdropType.MicaAlt : BackdropType.Mica;
        }
        else if (backdropType == typeof(TransparentBackdrop))
        {
            return BackdropType.Transparent;
        }
        else if (backdropType == typeof(AcrylicBackdrop))
        {
            var acrylic = (AcrylicBackdrop)systemBackdrop;
            return acrylic.Kind == DesktopAcrylicKind.Thin ? BackdropType.AcrylicThin : BackdropType.AcrylicBase;
        }
        else if (backdropType == typeof(DesktopAcrylicBackdrop))
        {
            return BackdropType.DesktopAcrylic;
        }
        else
        {
            return BackdropType.None;
        }
    }

    public void SetCurrentSystemBackdrop(BackdropType backdropType)
    {
        var systemBackdrop = GetSystemBackdrop(backdropType);
        CurrentBackdropType = backdropType;

        if (Settings.BackdropType != backdropType)
        {
            SetWindowBackdrop(systemBackdrop);
        }

        SetBackdropFallBackColorForWindows10(CurrentWindow);

        if (this.useAutoSave && Settings.BackdropType != backdropType)
        {
            Settings.BackdropType = backdropType;
            Settings?.Save();
        }
    }

    private void SetWindowBackdrop(SystemBackdrop systemBackdrop)
    {
        CurrentWindow.SystemBackdrop = systemBackdrop;
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
            var currentTheme = GeneralHelper.GetEnum<ElementTheme>(selectedTheme);
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
            var backdrop = GeneralHelper.GetEnum<BackdropType>(selectedBackdrop);
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
            var currentTheme = GeneralHelper.GetEnum<ElementTheme>(selectedTheme);
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
            var backdrop = GeneralHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetCurrentSystemBackdrop(backdrop);
        }
    }

    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetCurrentSystemBackdrop()).ToString();

        BackdropPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop).IsChecked = true;
    }

    public ElementTheme GetActualTheme()
    {
        return ActualTheme;
    }

    public ElementTheme GetRootTheme()
    {
        return RootTheme;
    }
}
