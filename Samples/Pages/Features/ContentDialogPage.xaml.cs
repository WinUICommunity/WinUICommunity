using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class ContentDialogPage : Page
{
    public ContentDialogPage()
    {
        this.InitializeComponent();
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        for (int i = 1; i <= 5; i++)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = $"Title {i}",
                Content = $"Content {i}",
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = XamlRoot
            };
            var result = await dialog.ShowAsyncQueue();
        }
    }
}
