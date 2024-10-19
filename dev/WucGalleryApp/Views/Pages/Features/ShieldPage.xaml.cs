using Windows.System;

namespace WucGalleryApp.Views;

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
