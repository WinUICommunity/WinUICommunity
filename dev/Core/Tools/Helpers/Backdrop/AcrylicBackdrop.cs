using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public class AcrylicBackdrop : SystemBackdrop
{
    DesktopAcrylicController acrylicController;
    public DesktopAcrylicKind Kind { get; set; }
    public AcrylicBackdrop(DesktopAcrylicKind acrylicKind)
    {
        this.Kind = acrylicKind;
    }
    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        acrylicController = new DesktopAcrylicController();
        acrylicController.Kind = Kind;

        // Set configuration.
        SystemBackdropConfiguration defaultConfig = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);
        acrylicController.SetSystemBackdropConfiguration(defaultConfig);

        // Add target.
        acrylicController.AddSystemBackdropTarget(connectedTarget);
    }
    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);
        acrylicController.RemoveSystemBackdropTarget(disconnectedTarget);
        acrylicController = null;
    }
}
