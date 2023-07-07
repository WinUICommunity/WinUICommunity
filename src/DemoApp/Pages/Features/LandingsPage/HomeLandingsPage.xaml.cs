using DemoApp.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity.DemoApp.Pages;

public sealed partial class HomeLandingsPage : Page
{
    public HomeLandingsPage()
    {
        this.InitializeComponent();
    }

    private void mainLandingsPage_Loaded(object sender, RoutedEventArgs e)
    {
        mainLandingsPage.GetDataAsync("DataModel/ControlInfoData.json", PathType.Relative);
    }

    private void mainLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (ControlInfoDataItem)args.ClickedItem;

        MainPage.Instance.Navigate(item.UniqueId);
    }
}
