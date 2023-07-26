using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUICommunity.DemoApp.Pages;
public sealed partial class mySectionPage : Page
{
    public mySectionPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var uniqueId = e.Parameter as string;
        sectionPage.GetData(NavigationManagerPage.Instance.jsonNavigationViewService.DataSource, uniqueId);
    }

    private void SectionPage_OnItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        NavigationManagerPage.Instance.jsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
