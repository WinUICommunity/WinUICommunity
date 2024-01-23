using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public sealed class AcrylicSystemBackdrop : SystemBackdrop
{
    public readonly DesktopAcrylicKind Kind;
    private DesktopAcrylicController acrylicController;

    public SystemBackdropConfiguration BackdropConfiguration { get; private set; }

    private Color _color;
    public Color TintColor
    {
        get { return _color; }
        set
        {
            _color = value;
            if (acrylicController != null)
            {
                acrylicController.TintColor = value;
            }
        }
    }
    public AcrylicSystemBackdrop(DesktopAcrylicKind desktopAcrylicKind)
    {
        Kind = desktopAcrylicKind;
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        acrylicController = new DesktopAcrylicController() { Kind = this.Kind };
        acrylicController.AddSystemBackdropTarget(connectedTarget);
        BackdropConfiguration = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);
        acrylicController.SetSystemBackdropConfiguration(BackdropConfiguration);
    }

    protected override void OnDefaultSystemBackdropConfigurationChanged(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
    {
        if (target != null)
            base.OnDefaultSystemBackdropConfigurationChanged(target, xamlRoot);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);

        if (acrylicController is not null)
        {
            acrylicController.RemoveSystemBackdropTarget(disconnectedTarget);
            acrylicController = null;
        }
    }
}
