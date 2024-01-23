using Microsoft.UI.Composition.SystemBackdrops;

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

        if (useAutoSave)
        {
            if (Settings.BackdropType != backdropType)
            {
                SetWindowSystemBackdrop(systemBackdrop);

                Settings.BackdropType = backdropType;
                Settings?.Save();
            }
        }
        else
        {
            SetWindowSystemBackdrop(systemBackdrop);
        }

        SetBackdropFallBackColorForUnSupportedOS();
    }

    private void SetBackdropFallBackColorForUnSupportedOS()
    {
        var currentBackdropType = GetBackdropType();
        if ((currentBackdropType == BackdropType.Mica ||
            currentBackdropType == BackdropType.MicaAlt) &&
            !MicaController.IsSupported())
        {
            SetBackdropFallBackColorForUnSupportedOSBase();
        }
        else if ((currentBackdropType == BackdropType.AcrylicBase ||
            currentBackdropType == BackdropType.AcrylicThin ||
            currentBackdropType == BackdropType.DesktopAcrylic) &&
            !DesktopAcrylicController.IsSupported())
        {
            SetBackdropFallBackColorForUnSupportedOSBase();
        }
        
    }
    private void SetBackdropFallBackColorForUnSupportedOSBase()
    {
        if (backdropFallBackColorForWindows10 != null)
        {
            var content = Window.Content;
            if (content != null)
            {
                var element = Window.Content as FrameworkElement;
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
