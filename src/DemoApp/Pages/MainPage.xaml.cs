using System;

using Microsoft.UI.Xaml.Controls;

using WinUICommunity;

namespace DemoApp.Pages;
public sealed partial class MainPage : Page
{
    internal static MainPage Instance { get; private set; }

    public MainPage()
    {
        this.InitializeComponent();
        Instance = this;

        App.Current.NavigationManager = NavigationManager.Initialize(NavigationViewControl, new NavigationViewOptions
        {
            DefaultPage = typeof(HomeLandingPage),
            SettingsPage = typeof(SettingsPage),
            JsonOptions = new JsonOptions
            {
                JsonFilePath = "DataModel/DemoData.json"
            }
        }, RootFrame, ControlsSearchBox);
    }

    public void Navigate(string uniqeId)
    {
        Type pageType = Type.GetType(uniqeId);
        RootFrame.Navigate(pageType);
    }
}
