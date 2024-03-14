using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class NavigationManagerPage : Page
{
    internal static NavigationManagerPage Instance { get; private set; }

    public IJsonNavigationViewService jsonNavigationViewService;

    public NavigationManagerPage()
    {
        this.InitializeComponent();
        Instance = this;
        Loaded += NavigationManagerPage_Loaded;
    }

    private void NavigationManagerPage_Loaded(object sender, RoutedEventArgs e)
    {
        InitNavigationView();
        InitNavigationViewWithJson();
    }

    private void InitNavigationView()
    {
        var pageService = new myPageService();
        pageService.SetDefaultPage(typeof(HomeLandingsPage));
        pageService.SetSettingsPage(typeof(GeneralPage));
        INavigationViewServiceEx navigationViewService;
        INavigationServiceEx navigationService;

        navigationService = new NavigationServiceEx(pageService);
        navigationService.Frame = shellFrame;
        navigationViewService = new NavigationViewServiceEx(navigationService, pageService);
        navigationViewService.Initialize(navigationView);
        navigationViewService.ConfigAutoSuggestBox(autoSuggestBox);
    }

    private void InitNavigationViewWithJson()
    {
        jsonNavigationViewService = new JsonNavigationViewService();
        jsonNavigationViewService.Initialize(NavigationViewControl, rootFrame);
        jsonNavigationViewService.ConfigJson("DataModel/AppData.json");
        jsonNavigationViewService.ConfigDefaultPage(typeof(HomeLandingsPage));
        jsonNavigationViewService.ConfigSettingsPage(typeof(GeneralPage));
        jsonNavigationViewService.ConfigSectionPage(typeof(mySectionPage));
        jsonNavigationViewService.ConfigAutoSuggestBox(controlsSearchBox);
    }
}
