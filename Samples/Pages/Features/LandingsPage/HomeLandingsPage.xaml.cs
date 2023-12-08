﻿using DemoApp;
using DemoApp.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WindowUI.DemoApp.Pages;

public sealed partial class HomeLandingsPage : Page
{
    public HomeLandingsPage()
    {
        this.InitializeComponent();
    }

    private void mainLandingsPage_Loaded(object sender, RoutedEventArgs e)
    {
        mainLandingsPage.GetData(App.Current.JsonNavigationViewService.DataSource);
    }

    private void mainLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.JsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
