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
            SetBackdropFallBackColorForUnSupportedOS();
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

    public void ConfigBackdropFallBackColorForUnSupportedOS(Brush brush)
    {
        backdropFallBackColorForWindows10 = brush;
    }

    public void ConfigTitleBar(TitleBarCustomization titleBarCustomization)
    {
        this.titleBarCustomization = titleBarCustomization;
        UpdateSystemCaptionButtonColors();
    }
}
