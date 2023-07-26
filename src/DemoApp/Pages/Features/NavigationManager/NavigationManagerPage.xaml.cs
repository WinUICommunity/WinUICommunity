using DemoApp.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace WinUICommunity.DemoApp.Pages;

public sealed partial class NavigationManagerPage : Page
{
    public NavigationManagerPage()
    {
        this.InitializeComponent();
    }
    private void tabviewItem1_Loaded(object sender, RoutedEventArgs e)
    {
        var pageService = new myPageService();
        pageService.SetDefaultPage(typeof(HomeLandingsPage));
        pageService.SetSettingsPage(typeof(GeneralPage));
        INavigationViewService navigationViewService;
        INavigationService navigationService;

        navigationService = new NavigationService(pageService);
        navigationService.Frame = shellFrame;
        navigationViewService = new NavigationViewService(navigationService, pageService);
        navigationViewService.Initialize(navigationView);
        navigationViewService.ConfigAutoSuggestBox(autoSuggestBox);
    }

    private void tabViewItem2_Loaded(object sender, RoutedEventArgs e)
    {
        IJsonNavigationViewService jsonNavigationViewService;
        jsonNavigationViewService = new JsonNavigationViewService();
        jsonNavigationViewService.Initialize(NavigationViewControl, rootFrame);
        jsonNavigationViewService.ConfigJson("DataModel/AppData.json");
        jsonNavigationViewService.ConfigDefaultPage(typeof(HomeLandingsPage));
        jsonNavigationViewService.ConfigSettingsPage(typeof(GeneralPage));
        jsonNavigationViewService.ConfigAutoSuggestBox(controlsSearchBox);
    }
}
