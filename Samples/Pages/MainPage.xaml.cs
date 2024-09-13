using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUICommunityGallery.Pages;
public sealed partial class MainPage : Page
{
    internal static MainPage Instance { get; private set; }

    public MainPage()
    {
        this.InitializeComponent();
        App.CurrentWindow.ExtendsContentIntoTitleBar = true;
        App.CurrentWindow.SetTitleBar(AppTitleBar);
        Instance = this;

        App.Current.JsonNavigationViewService.Initialize(NavView, NavFrame);
        App.Current.JsonNavigationViewService.ConfigJson("DataModel/AppData.json");
        App.Current.JsonNavigationViewService.ConfigDefaultPage(typeof(HomeLandingPage));
        App.Current.JsonNavigationViewService.ConfigSettingsPage(typeof(SettingsPage));
        App.Current.JsonNavigationViewService.ConfigSectionPage(typeof(DemoSectionPage));
        App.Current.JsonNavigationViewService.ConfigAutoSuggestBox(ControlsSearchBox);
    }

    private void AppTitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (NavFrame.CanGoBack)
        {
            NavFrame.GoBack();
        }
    }

    private void AppTitleBar_PaneButtonClick(object sender, RoutedEventArgs e)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void NavFrame_Navigated(object sender, NavigationEventArgs e)
    {
        AppTitleBar.IsBackButtonVisible = NavFrame.CanGoBack;
    }
}
