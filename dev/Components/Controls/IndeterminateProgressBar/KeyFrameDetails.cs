using Microsoft.UI.Xaml.Media.Animation;

namespace WinUICommunity;

internal class KeyFrameDetails
{
    public KeyTime KeyFrameTime { get; set; }
    public List<DoubleKeyFrame> KeyFrames { get; set; }
}
