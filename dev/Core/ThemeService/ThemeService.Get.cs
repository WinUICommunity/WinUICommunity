namespace WinUICommunity;
public partial class ThemeService
{
    public ElementTheme GetElementTheme()
    {
        return RootTheme;
    }

    public ElementTheme GetActualTheme()
    {
        return ActualTheme;
    }

    public SystemBackdrop GetSystemBackdrop()
    {
        return CurrentSystemBackdrop;
    }

    public SystemBackdrop GetSystemBackdrop(BackdropType backdropType)
    {
        switch (backdropType)
        {
            case BackdropType.None:
                return null;
            case BackdropType.Mica:
                return new MicaSystemBackdrop(Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base);
            case BackdropType.MicaAlt:
                return new MicaSystemBackdrop(Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt);
            case BackdropType.DesktopAcrylic:
                return new DesktopAcrylicBackdrop();
            case BackdropType.AcrylicBase:
                return new AcrylicSystemBackdrop(Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicKind.Base);
            case BackdropType.AcrylicThin:
                return new AcrylicSystemBackdrop(Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicKind.Thin);
            case BackdropType.Transparent:
                return new TransparentBackdrop();
            default:
                return null;
        }
    }

    public BackdropType GetBackdropType()
    {
        return CurrentBackdropType;
    }

    public BackdropType GetBackdropType(SystemBackdrop systemBackdrop)
    {
        var backdropType = systemBackdrop?.GetType();
        if (systemBackdrop is MicaSystemBackdrop mica)
        {
            return mica.kind == Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base ? BackdropType.Mica : BackdropType.MicaAlt;
        }
        else if (backdropType == typeof(TransparentBackdrop))
        {
            return BackdropType.Transparent;
        }
        else if (systemBackdrop is AcrylicSystemBackdrop acrylic)
        {
            return acrylic.kind == Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicKind.Base ? BackdropType.AcrylicBase : BackdropType.AcrylicThin;
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

    private SystemBackdrop GetSystemBackdropFromLocalConfig(BackdropType backdropType, bool ForceBackdrop)
    {
        BackdropType currentBackdrop = this.useAutoSave ? Settings.BackdropType : backdropType;
        return ForceBackdrop ? GetSystemBackdrop(backdropType) : GetSystemBackdrop(currentBackdrop);
    }

    private ElementTheme GetElementThemeFromLocalConfig(ElementTheme theme, bool forceTheme)
    {
        var currentTheme = Settings.ElementTheme;
        return forceTheme ? theme : currentTheme;
    }
}
