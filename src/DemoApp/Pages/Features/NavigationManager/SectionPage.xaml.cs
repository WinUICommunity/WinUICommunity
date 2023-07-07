using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUICommunity.DemoApp.Pages;

public sealed partial class SectionPage : Page
{
    public SectionPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        sectionPage.GetDataAsync(e);
    }

    private void SectionPage_OnItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (ControlInfoDataItem)args.ClickedItem;
        NavigationManagerPage.Instance.NavWithJson2.NavigateForJson(typeof(ItemPage), item.UniqueId);
    }
}
