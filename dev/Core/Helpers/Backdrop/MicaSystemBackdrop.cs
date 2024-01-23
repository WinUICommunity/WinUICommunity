using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public sealed class MicaSystemBackdrop : SystemBackdrop
{
    public readonly MicaKind Kind;
    private MicaController micaController;

    public SystemBackdropConfiguration BackdropConfiguration { get; private set; }

    private Color _color;
    public Color TintColor
    {
        get { return _color; }
        set
        {
            _color = value;
            if (micaController != null)
            {
                micaController.TintColor = value;
            }
        }
    }
    public MicaSystemBackdrop(MicaKind micaKind)
    {
        Kind = micaKind;
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        micaController = new MicaController() { Kind = this.Kind };
        micaController.AddSystemBackdropTarget(connectedTarget);
        BackdropConfiguration = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);
        micaController.SetSystemBackdropConfiguration(BackdropConfiguration);
    }

    protected override void OnDefaultSystemBackdropConfigurationChanged(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
    {
        if (target != null)
            base.OnDefaultSystemBackdropConfigurationChanged(target, xamlRoot);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);

        if (micaController is not null)
        {
            micaController.RemoveSystemBackdropTarget(disconnectedTarget);
            micaController = null;
        }
    }
}
