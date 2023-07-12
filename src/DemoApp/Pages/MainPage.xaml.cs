using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using WinUICommunity;

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

        App.Current.NavigationManager = NavigationManager.Initialize(NavView, new NavigationViewOptions
        {
            DefaultPage = typeof(HomeLandingPage),
            SettingsPage = typeof(SettingsPage),
            JsonOptions = new JsonOptions
            {
                JsonFilePath = "DataModel/DemoData.json"
            }
        }, NavFrame, ControlsSearchBox);
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

    public void Navigate(string uniqeId)
    {
        Type pageType = Type.GetType(uniqeId);
        NavFrame.Navigate(pageType);
    }
}
