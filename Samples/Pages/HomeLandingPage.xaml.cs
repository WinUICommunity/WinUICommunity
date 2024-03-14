using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using WinUICommunity;

namespace WinUICommunityGallery.Pages;
public sealed partial class HomeLandingPage : Page
{
    public HomeLandingPage()
    {
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        landingsPage.GetData(App.Current.JsonNavigationViewService.DataSource);
        landingsPage.OrderBy(i => i.Title);
    }

    private void LandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.JsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
