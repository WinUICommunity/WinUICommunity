using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public sealed class MicaSystemBackdrop : SystemBackdrop
{
    public readonly MicaKind micaBackdropKind;
    private ISystemBackdropControllerWithTargets systemBackdropController;

    public SystemBackdropConfiguration BackdropConfiguration { get; private set; }

    public MicaSystemBackdrop(MicaKind micaKind)
    {
        micaBackdropKind = micaKind;
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        systemBackdropController = new MicaController() { Kind = micaBackdropKind };
        systemBackdropController.AddSystemBackdropTarget(connectedTarget);
        BackdropConfiguration = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);
        systemBackdropController.SetSystemBackdropConfiguration(BackdropConfiguration);
    }

    protected override void OnDefaultSystemBackdropConfigurationChanged(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
    {
        if (target != null)
            base.OnDefaultSystemBackdropConfigurationChanged(target, xamlRoot);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);

        if (systemBackdropController is not null)
        {
            systemBackdropController.RemoveSystemBackdropTarget(disconnectedTarget);
            systemBackdropController = null;
        }
    }
}
