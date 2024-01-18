using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace WinUICommunity;
public class MicaBackdrop : SystemBackdrop
{
    public MicaController micaController = new MicaController();

    private MicaKind kind;
    public MicaKind Kind
    {
        get { return kind; }
        set
        {
            kind = value;
            if (micaController != null)
            {
                micaController.Kind = value;
            }
        }
    }


    private Color tintColor;
    public Color TintColor
    {
        get { return tintColor; }
        set
        {
            tintColor = value;
            if (micaController != null)
            {
                micaController.TintColor = value;
            }
        }
    }


    private Color fallbackColor;
    public Color FallbackColor
    {
        get { return fallbackColor; }
        set
        {
            fallbackColor = value;
            if (micaController != null)
            {
                micaController.FallbackColor = value;
            }
        }
    }


    private float tintOpacity;
    public float TintOpacity
    {
        get { return tintOpacity; }
        set
        {
            tintOpacity = value;
            if (micaController != null)
            {
                micaController.TintOpacity = value;
            }
        }
    }


    private float luminosityOpacity;
    public float LuminosityOpacity
    {
        get { return luminosityOpacity; }
        set
        {
            luminosityOpacity = value;
            if (micaController != null)
            {
                micaController.LuminosityOpacity = value;
            }
        }
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        // Set configuration.
        SystemBackdropConfiguration defaultConfig = GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot);
        micaController.SetSystemBackdropConfiguration(defaultConfig);

        // Add target.
        micaController.AddSystemBackdropTarget(connectedTarget);
    }
    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);
        micaController.RemoveSystemBackdropTarget(disconnectedTarget);
        micaController = null;
    }
}
