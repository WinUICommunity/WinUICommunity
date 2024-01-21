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
            CurrentBackdropType = GetBackdropType(systemBackdrop);

            SetWindowSystemBackdrop(systemBackdrop);
            SetBackdropFallBackColorForWindows10(Window);

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
