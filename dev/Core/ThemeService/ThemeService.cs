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
    public Window Window { get; set; }
    public SystemBackdrop CurrentSystemBackdrop { get; set; }
    private bool changeThemeWithoutSave = false;
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
        if (Window == null)
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
            var systemBackdrop = GetSystemBackdropFromLocalConfig(backdropType, force);
            CurrentSystemBackdrop = systemBackdrop;
            SetWindowSystemBackdrop(systemBackdrop);
            SetBackdropFallBackColorForWindows10(Window);

            CurrentBackdropType = GetBackdropType(systemBackdrop);
        }
        else
        {
            CurrentBackdropType = BackdropType.None;
        }
    }

    public void SetBackdropTintColor(Color color)
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaBackdrop mica)
            {
                mica.TintColor = color;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.TintColor = color;
            }
        }

        if (this.useAutoSave && Settings.BackdropTintColor != color)
        {
            Settings.BackdropTintColor = color;
            Settings?.Save();
        }
    }

    public void SetBackdropFallBackColor(Color color)
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaBackdrop mica)
            {
                mica.FallbackColor = color;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.FallbackColor = color;
            }
        }

        if (this.useAutoSave && Settings.BackdropFallBackColor != color)
        {
            Settings.BackdropFallBackColor = color;
            Settings?.Save();
        }
    }

    public void SetBackdropTintOpacity(float opacity)
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaBackdrop mica)
            {
                mica.TintOpacity = opacity;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.TintOpacity = opacity;
            }
        }

        if (this.useAutoSave && Settings.BackdropTintOpacity != opacity)
        {
            Settings.BackdropTintOpacity = opacity;
            Settings?.Save();
        }
    }

    public void SetBackdropLuminosityOpacity(float opacity)
    {
        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaBackdrop mica)
            {
                mica.LuminosityOpacity = opacity;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.LuminosityOpacity = opacity;
            }
        }

        if (this.useAutoSave && Settings.BackdropLuminosityOpacity != opacity)
        {
            Settings.BackdropLuminosityOpacity = opacity;
            Settings?.Save();
        }
    }

    public void ConfigBackdropTintColor(Color color, bool force = false)
    {
        if (useAutoSave && Settings.IsBackdropTintColorFirstRun)
        {
            Settings.BackdropTintColor = color;
            Settings.IsBackdropTintColorFirstRun = false;
            Settings.Save();
        }

        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            var colorFromConfig = GetBackdropTintColorFromLocalConfig(color, force);
            if (backdrop is MicaBackdrop mica)
            {
                mica.TintColor = colorFromConfig;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.TintColor = colorFromConfig;
            }
        }
    }

    public void ConfigBackdropFallBackColor(Color color, bool force = false)
    {
        if (useAutoSave && Settings.IsBackdropFallBackColorFirstRun)
        {
            Settings.BackdropFallBackColor = color;
            Settings.IsBackdropFallBackColorFirstRun = false;
            Settings.Save();
        }

        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            Color colorFromConfig = GetBackdropFallBackColorFromLocalConfig(color, force);

            if (backdrop is MicaBackdrop mica)
            {
                mica.FallbackColor = colorFromConfig;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.FallbackColor = colorFromConfig;
            }

            if (!force)
            {
                if (this.useAutoSave && Settings.BackdropFallBackColor != color)
                {
                    Settings.BackdropFallBackColor = color;
                    Settings?.Save();
                }
            }
        }
    }

    public void ConfigBackdropTintOpacity(float opacity, bool force = false)
    {
        if (useAutoSave && Settings.IsBackdropTintOpacityFirstRun)
        {
            Settings.BackdropTintOpacity = opacity;
            Settings.IsBackdropTintOpacityFirstRun = false;
            Settings.Save();
        }

        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            float opacityFromConfig = GetBackdropTintOpacityFromLocalConfig(opacity, force);

            if (backdrop is MicaBackdrop mica)
            {
                mica.TintOpacity = opacityFromConfig;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.TintOpacity = opacityFromConfig;
            }

            if (!force)
            {
                if (this.useAutoSave && Settings.BackdropTintOpacity != opacity)
                {
                    Settings.BackdropTintOpacity = opacity;
                    Settings?.Save();
                }
            }
        }
    }

    public void ConfigBackdropLuminosityOpacity(float opacity, bool force = false)
    {
        if (useAutoSave && Settings.IsBackdropLuminosityOpacityFirstRun)
        {
            Settings.BackdropLuminosityOpacity = opacity;
            Settings.IsBackdropLuminosityOpacityFirstRun = false;
            Settings.Save();
        }

        var backdrop = GetSystemBackdrop();
        if (backdrop != null)
        {
            float opacityFromConfig = GetBackdropLuminosityOpacityFromLocalConfig(opacity, force);

            if (backdrop is MicaBackdrop mica)
            {
                mica.LuminosityOpacity = opacityFromConfig;
            }
            else if (backdrop is AcrylicBackdrop acrylic)
            {
                acrylic.LuminosityOpacity = opacityFromConfig;
            }

            if (!force)
            {
                if (this.useAutoSave && Settings.BackdropLuminosityOpacity != opacity)
                {
                    Settings.BackdropLuminosityOpacity = opacity;
                    Settings?.Save();
                }
            }
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
            SetElementTheme(GetElementThemeFromLocalConfig(elementTheme, force));
        }
        else
        {
            SetElementTheme(elementTheme);
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

    public BackdropType GetBackdropType()
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
                return new TransparentBackdrop() { Kind = TransparentKind.Base };
            case BackdropType.TransparentFull:
                return new TransparentBackdrop() { Kind = TransparentKind.Full };
            default:
                return null;
        }
    }

    public SystemBackdrop GetSystemBackdrop()
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
            var transparent = (TransparentBackdrop)systemBackdrop;
            return transparent.Kind == TransparentKind.Full ? BackdropType.TransparentFull : BackdropType.Transparent;
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

    public void SetBackdropType(BackdropType backdropType)
    {
        var systemBackdrop = GetSystemBackdrop(backdropType);
        CurrentBackdropType = backdropType;

        if (Settings.BackdropType != backdropType)
        {
            SetWindowSystemBackdrop(systemBackdrop);
        }

        SetBackdropFallBackColorForWindows10(Window);

        if (this.useAutoSave && Settings.BackdropType != backdropType)
        {
            Settings.BackdropType = backdropType;
            Settings?.Save();
        }
    }

    private void SetWindowSystemBackdrop(SystemBackdrop systemBackdrop)
    {
        Window.SystemBackdrop = systemBackdrop;
    }

    private SystemBackdrop GetSystemBackdropFromLocalConfig(BackdropType backdropType, bool ForceBackdrop)
    {
        BackdropType currentBackdrop = this.useAutoSave ? Settings.BackdropType : backdropType;
        return ForceBackdrop ? GetSystemBackdrop(backdropType) : GetSystemBackdrop(currentBackdrop);
    }

    private Color GetBackdropTintColorFromLocalConfig(Color color, bool ForceColor)
    {
        Color currentColor = this.useAutoSave ? Settings.BackdropTintColor : color;
        return ForceColor ? color : currentColor;
    }

    private Color GetBackdropFallBackColorFromLocalConfig(Color color, bool ForceColor)
    {
        Color currentColor = this.useAutoSave ? Settings.BackdropFallBackColor : color;
        return ForceColor ? color : currentColor;
    }
    private float GetBackdropTintOpacityFromLocalConfig(float opacity, bool ForceOpacity)
    {
        float currentOpacity = this.useAutoSave ? Settings.BackdropTintOpacity : opacity;
        return ForceOpacity ? opacity : currentOpacity;
    }

    private float GetBackdropLuminosityOpacityFromLocalConfig(float opacity, bool ForceOpacity)
    {
        float currentOpacity = this.useAutoSave ? Settings.BackdropLuminosityOpacity : opacity;
        return ForceOpacity ? opacity : currentOpacity;
    }
    private ElementTheme GetElementThemeFromLocalConfig(ElementTheme theme, bool forceTheme)
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
        if (Window != null)
        {
            UpdateSystemCaptionButton(Window);
        }
    }

    public void SetElementTheme(ElementTheme elementTheme)
    {
        changeThemeWithoutSave = false;
        if (RootTheme != elementTheme)
        {
            RootTheme = elementTheme;
        }
    }

    public void SetElementThemeWithoutSave(ElementTheme elementTheme)
    {
        changeThemeWithoutSave = true;
        if (RootTheme != elementTheme)
        {
            RootTheme = elementTheme;
        }
    }

    public ElementTheme GetElementTheme()
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
            SetElementTheme(currentTheme);
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
            SetBackdropType(backdrop);
        }
    }

    public void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();

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
            SetElementTheme(currentTheme);
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
            SetBackdropType(backdrop);
        }
    }

    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();

        BackdropPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop).IsChecked = true;
    }

    public ElementTheme GetActualTheme()
    {
        return ActualTheme;
    }
}
