using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUICommunity.DemoApp.Pages;

public sealed partial class ItemPage : Page
{
    public ItemPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        itemPage.GetDataAsync(e);
    }
}
