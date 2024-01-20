namespace WinUICommunity;
public partial class ThemeService
{
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
            if (systemBackdrop is AcrylicBase)
            {
                SetWindowSystemBackdrop(null);
                new AcrylicBackdropHelper(Window).TrySetAcrylicBackdrop(Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicKind.Base);
            }
            else if (systemBackdrop is AcrylicThin)
            {
                SetWindowSystemBackdrop(null);
                new AcrylicBackdropHelper(Window).TrySetAcrylicBackdrop(Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicKind.Thin);
            }
            else
            {
                SetWindowSystemBackdrop(systemBackdrop);
            }
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
