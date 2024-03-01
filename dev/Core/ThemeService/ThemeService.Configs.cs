namespace WinUICommunity;
public partial class ThemeService
{
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
        }

        if (backdropType != BackdropType.None)
        {
            var systemBackdrop = GetSystemBackdropFromLocalConfig(backdropType, force);

            SetWindowSystemBackdrop(systemBackdrop);
        }
    }

    public void ConfigTintColor(Color color, bool force)
    {
        if (useAutoSave && Settings.IsBackdropTintColorFirstRun)
        {
            Settings.BackdropTintColor = color;
            Settings.IsBackdropTintColorFirstRun = false;
            Settings.Save();
        }

        var tintColor = GetBackdropTintColorFromLocalConfig(color, force);

        SetBackdropTintColor(tintColor);
    }
    public void ConfigTintColor()
    {
        var systemBackdrop = Window.SystemBackdrop;
        if (systemBackdrop != null)
        {
            if (IsDarkTheme())
            {
                switch (GetBackdropType())
                {
                    case BackdropType.Mica:
                        ConfigTintColor(MicaSystemBackdrop.Default_TintColor_Dark, false);
                        break;
                    case BackdropType.MicaAlt:
                        ConfigTintColor(MicaSystemBackdrop.Default_TintColor_MicaAlt_Dark, false);
                        break;
                    case BackdropType.AcrylicThin:
                    case BackdropType.AcrylicBase:
                        ConfigTintColor(AcrylicSystemBackdrop.Default_TintColor_Dark, false);
                        break;
                }
            }
            else
            {
                switch (GetBackdropType())
                {
                    case BackdropType.Mica:
                        ConfigTintColor(MicaSystemBackdrop.Default_TintColor_Light, false);
                        break;
                    case BackdropType.MicaAlt:
                        ConfigTintColor(MicaSystemBackdrop.Default_TintColor_MicaAlt_Light, false);
                        break;
                    case BackdropType.AcrylicThin:
                    case BackdropType.AcrylicBase:
                        ConfigTintColor(AcrylicSystemBackdrop.Default_TintColor_Light, false);
                        break;
                }
            }

        }
    }
    public void ConfigFallbackColor(Color color, bool force)
    {
        if (useAutoSave && Settings.IsBackdropFallBackColorFirstRun)
        {
            Settings.BackdropFallBackColor = color;
            Settings.IsBackdropFallBackColorFirstRun = false;
            Settings.Save();
        }

        var tintColor = GetBackdropFallbackColorFromLocalConfig(color, force);

        SetBackdropFallbackColor(tintColor);
    }

    public void ConfigFallbackColor()
    {
        var systemBackdrop = Window.SystemBackdrop;
        if (systemBackdrop != null)
        {
            if (IsDarkTheme())
            {
                switch (GetBackdropType())
                {
                    case BackdropType.MicaAlt:
                        ConfigFallbackColor(MicaSystemBackdrop.Default_FallbackColor_MicaAlt_Dark, false);
                        break;
                    case BackdropType.AcrylicThin:
                    case BackdropType.AcrylicBase:
                        ConfigFallbackColor(AcrylicSystemBackdrop.Default_FallbackColor_Dark, false);
                        break;
                }
            }
            else
            {
                switch (GetBackdropType())
                {
                    case BackdropType.MicaAlt:
                        ConfigFallbackColor(MicaSystemBackdrop.Default_FallbackColor_MicaAlt_Light, false);
                        break;
                    case BackdropType.AcrylicThin:
                    case BackdropType.AcrylicBase:
                        ConfigFallbackColor(AcrylicSystemBackdrop.Default_FallbackColor_Light, false);
                        break;
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

    public void ConfigTitleBar(TitleBarCustomization titleBarCustomization)
    {
        this.titleBarCustomization = titleBarCustomization;
        UpdateSystemCaptionButtonColors();
    }
}
