using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using WinUICommunity;

namespace DemoApp.Pages;
public sealed partial class HomeLandingPage : Page
{
    public HomeLandingPage()
    {
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        allLandingsPage.GetDataAsync("DataModel/DemoData.json", PathType.Relative);
    }

    private void allLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (ControlInfoDataItem)args.ClickedItem;
        Type pageType = Type.GetType(item.UniqueId);

        if (pageType != null)
        {
            object parameter = null;
            App.Current.NavigationManager.NavigateForJson(pageType, parameter);
        }
    }

}
