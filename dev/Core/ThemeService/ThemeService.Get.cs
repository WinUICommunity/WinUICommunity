using Microsoft.UI.Composition.SystemBackdrops;

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
                return new MicaBackdrop();
            case BackdropType.MicaAlt:
                return new MicaBackdrop { Kind = MicaKind.BaseAlt };
            case BackdropType.DesktopAcrylic:
                return new DesktopAcrylicBackdrop();
            case BackdropType.AcrylicBase:
                return new AcrylicBase();
            case BackdropType.AcrylicThin:
                return new AcrylicThin();
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
        if (backdropType == typeof(MicaBackdrop))
        {
            var mica = (MicaBackdrop)systemBackdrop;
            return mica.Kind == MicaKind.BaseAlt ? BackdropType.MicaAlt : BackdropType.Mica;
        }
        else if (backdropType == typeof(TransparentBackdrop))
        {
            return BackdropType.Transparent;
        }
        else if (backdropType == typeof(AcrylicBase))
        {
            return BackdropType.AcrylicBase;
        }
        else if (backdropType == typeof(AcrylicThin))
        {
            return BackdropType.AcrylicThin;
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
