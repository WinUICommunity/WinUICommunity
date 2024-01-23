using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public class MicaSystemBackdrop : MicaBackdrop
{
    public MicaController micaController;
    public MicaKind kind;
    public MicaSystemBackdrop(MicaKind kind)
    {
        this.kind = kind;
        micaController = new MicaController();
    }

    public void Disconnect()
    {
        micaController.RemoveAllSystemBackdropTargets();
        micaController.ResetProperties();
        micaController.Dispose();
        micaController = null;
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);
        SystemBackdropConfiguration defaultConfig = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);

        micaController.SetSystemBackdropConfiguration(defaultConfig);
        micaController.AddSystemBackdropTarget(connectedTarget);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);
        micaController.RemoveSystemBackdropTarget(disconnectedTarget);
        micaController = null;
    }

    protected override void OnDefaultSystemBackdropConfigurationChanged(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
    {
        if (target != null)
        {
            base.OnDefaultSystemBackdropConfigurationChanged(target, xamlRoot);
        }
    }
}
