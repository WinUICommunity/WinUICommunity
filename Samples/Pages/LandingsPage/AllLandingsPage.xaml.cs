using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class AllLandingsPage : Page
{
    public AllLandingsPage()
    {
        this.InitializeComponent();
    }

    private void allLandingsPage_Loaded(object sender, RoutedEventArgs e)
    {
        allLandingsPage.GetData(App.Current.JsonNavigationViewService.DataSource);
    }

    private void allLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.JsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
