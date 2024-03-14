using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class RainbowFramePage : Page
{
    private RainbowFrame rainbowFrameHelper;
    public RainbowFramePage()
    {
        this.InitializeComponent();
        rainbowFrameHelper = new RainbowFrame();
        rainbowFrameHelper.Initialize(App.currentWindow);
    }

    private void btnFixed_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        rainbowFrameHelper?.StopRainbowFrame();
        rainbowFrameHelper.ChangeFrameColor(Colors.Red);
    }

    private void btnReset_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        rainbowFrameHelper?.ResetFrameColorToDefault();
    }

    private void btnRainbow_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        rainbowFrameHelper?.StopRainbowFrame();
        rainbowFrameHelper?.StartRainbowFrame();
    }

    private void nbEffectSpeed_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        rainbowFrameHelper?.UpdateEffectSpeed((int)args.NewValue);
    }
}
