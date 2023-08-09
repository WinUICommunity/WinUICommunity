using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using WinUICommunity.DemoApp.Pages;

namespace DemoApp.Pages;
public sealed partial class MainPage : Page
{
    internal static MainPage Instance { get; private set; }

    public MainPage()
    {
        this.InitializeComponent();
        appTitleBar.Window = App.currentWindow;
        appTitleBar.Title = App.Current.Title;
        Instance = this;

        App.Current.JsonNavigationViewService.Initialize(NavView, NavFrame);
        App.Current.JsonNavigationViewService.ConfigJson("DataModel/DemoData.json");
        App.Current.JsonNavigationViewService.ConfigDefaultPage(typeof(HomeLandingPage));
        App.Current.JsonNavigationViewService.ConfigSettingsPage(typeof(SettingsPage));
        App.Current.JsonNavigationViewService.ConfigSectionPage(typeof(DemoSectionPage));
        App.Current.JsonNavigationViewService.ConfigAutoSuggestBox(ControlsSearchBox);
    }

    private void appTitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (NavFrame.CanGoBack)
        {
            NavFrame.GoBack();
        }
    }

    private void appTitleBar_PaneButtonClick(object sender, RoutedEventArgs e)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void NavFrame_Navigated(object sender, NavigationEventArgs e)
    {
        appTitleBar.IsBackButtonVisible = NavFrame.CanGoBack;
    }
}
