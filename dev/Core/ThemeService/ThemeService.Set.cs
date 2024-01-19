namespace WinUICommunity;
public partial class ThemeService
{
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

    private void SetWindowSystemBackdrop(SystemBackdrop systemBackdrop)
    {
        Window.SystemBackdrop = systemBackdrop;
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
}
