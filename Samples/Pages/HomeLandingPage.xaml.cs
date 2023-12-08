﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using WindowUI;

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
        allLandingsPage.GetData(App.Current.JsonNavigationViewService.DataSource);
        allLandingsPage.OrderBy(i => i.Title);
    }

    private void allLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.JsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
