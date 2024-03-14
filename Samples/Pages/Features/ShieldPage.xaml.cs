using System;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

namespace WinUICommunityGallery.Pages;

public sealed partial class ShieldPage : Page
{
    public ShieldPage()
    {
        this.InitializeComponent();
    }

    private async void Shield_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/winUICommunity/WinUICommunity"));
    }
}
