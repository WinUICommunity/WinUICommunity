using Microsoft.UI.Composition.SystemBackdrops;

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
    public void ConfigBackdropTintColor()
    {
        var backdrop = ResetBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaController mica)
            {
                ConfigBackdropTintColor(mica.TintColor);
            }
            else if (backdrop is DesktopAcrylicController acrylic)
            {
                ConfigBackdropTintColor(acrylic.TintColor);
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
    public void ConfigBackdropFallBackColor()
    {
        var backdrop = ResetBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaController mica)
            {
                ConfigBackdropFallBackColor(mica.FallbackColor);
            }
            else if (backdrop is DesktopAcrylicController acrylic)
            {
                ConfigBackdropFallBackColor(acrylic.FallbackColor);
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
    public void ConfigBackdropTintOpacity()
    {
        var backdrop = ResetBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaController mica)
            {
                ConfigBackdropTintOpacity(mica.TintOpacity);
            }
            else if (backdrop is DesktopAcrylicController acrylic)
            {
                ConfigBackdropTintOpacity(acrylic.TintOpacity);
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
    public void ConfigBackdropLuminosityOpacity()
    {
        var backdrop = ResetBackdrop();
        if (backdrop != null)
        {
            if (backdrop is MicaController mica)
            {
                ConfigBackdropLuminosityOpacity(mica.LuminosityOpacity);
            }
            else if (backdrop is DesktopAcrylicController acrylic)
            {
                ConfigBackdropLuminosityOpacity(acrylic.LuminosityOpacity);
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
}
