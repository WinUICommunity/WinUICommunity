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
                return new AcrylicBackdrop() { Kind = DesktopAcrylicKind.Base };
            case BackdropType.AcrylicThin:
                return new AcrylicBackdrop() { Kind = DesktopAcrylicKind.Thin };
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

    public float GetDefaultBackdropTintOpacity()
    {
        var controller = ResetBackdrop();
        if (controller is MicaController mica)
        {
            return mica.TintOpacity;
        }
        else if (controller is DesktopAcrylicController acrylic)
        {
            return acrylic.TintOpacity;
        }
        return 0.5f;
    }

    public float GetDefaultBackdropLuminosityOpacity()
    {
        var controller = ResetBackdrop();
        if (controller is MicaController mica)
        {
            return mica.LuminosityOpacity;
        }
        else if (controller is DesktopAcrylicController acrylic)
        {
            return acrylic.LuminosityOpacity;
        }
        return 1f;
    }

    public Color GetDefaultBackdropTintColor()
    {
        var controller = ResetBackdrop();
        if (controller is MicaController mica)
        {
            return mica.TintColor;
        }
        else if (controller is DesktopAcrylicController acrylic)
        {
            return acrylic.TintColor;
        }
        return Colors.Transparent;
    }

    public Color GetDefaultBackdropFallBackColor()
    {
        var controller = ResetBackdrop();
        if (controller is MicaController mica)
        {
            return mica.FallbackColor;
        }
        else if (controller is DesktopAcrylicController acrylic)
        {
            return acrylic.FallbackColor;
        }
        return Colors.Transparent;
    }
}
