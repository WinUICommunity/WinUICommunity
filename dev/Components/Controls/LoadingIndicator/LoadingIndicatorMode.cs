using System.ComponentModel;

namespace WinUICommunity;
public enum LoadingIndicatorMode
{
    [Description("LoadingIndicatorWaveStyle")]
    Wave,

    [Description("LoadingIndicatorArcsStyle")]
    Arcs,

    [Description("LoadingIndicatorArcsRingStyle")]
    ArcsRing,

    [Description("LoadingIndicatorDoubleBounceStyle")]
    DoubleBounce,

    [Description("LoadingIndicatorFlipPlaneStyle")]
    FlipPlane,

    [Description("LoadingIndicatorPulseStyle")]
    Pulse,

    [Description("LoadingIndicatorRingStyle")]
    Ring,

    [Description("LoadingIndicatorThreeDotsStyle")]
    ThreeDots
}
