using Microsoft.UI.Xaml.Controls;
using Windows.UI;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class CompositionHelperPage : Page
{
    public CompositionHelperPage()
    {
        this.InitializeComponent();
        CompositionHelper.MakeLongShadow(188, 0.3f, TextBlockElement, ShadowElement, Color.FromArgb(255, 250, 110, 93));
    }
}
