using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public class AcrylicSystemBackdrop : DesktopAcrylicBackdrop
{
    public DesktopAcrylicController acrylicController;
    public DesktopAcrylicKind kind;
    public AcrylicSystemBackdrop(DesktopAcrylicKind kind)
    {
        this.kind = kind;
    }

    public void Disconnect()
    {
        acrylicController.RemoveAllSystemBackdropTargets();
        acrylicController.ResetProperties();
        acrylicController.Dispose();
        acrylicController = null;
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);
        SystemBackdropConfiguration defaultConfig = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);

        acrylicController.SetSystemBackdropConfiguration(defaultConfig);
        acrylicController.AddSystemBackdropTarget(connectedTarget);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);
        acrylicController.RemoveSystemBackdropTarget(disconnectedTarget);
        acrylicController = null;
    }

    protected override void OnDefaultSystemBackdropConfigurationChanged(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
    {
        if (target != null)
        {
            base.OnDefaultSystemBackdropConfigurationChanged(target, xamlRoot);
        }
    }
}

