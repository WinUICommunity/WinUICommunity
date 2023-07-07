using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace WinUICommunity.DemoApp.Pages;

public sealed partial class NavigationManagerPage : Page
{
    internal static NavigationManagerPage Instance { get; private set; }
    public NavigationManager NavWithJson2 { get; set; }
    public NavigationManagerPage()
    {
        this.InitializeComponent();
        Instance = this;
    }
    private void tabviewItem1_Loaded(object sender, RoutedEventArgs e)
    {
        new NavigationManager(navigationView, new NavigationViewOptions
        {
            DefaultPage = typeof(HomeLandingsPage),
            SettingsPage = typeof(GeneralPage)
        }, shellFrame, autoSuggestBox);
    }

    private void tabViewItem2_Loaded(object sender, RoutedEventArgs e)
    {
        new NavigationManager(NavigationViewControl, new NavigationViewOptions
        {
            DefaultPage = typeof(HomeLandingsPage),
            SettingsPage = typeof(GeneralPage),
            JsonOptions = new JsonOptions
            {
                HasInfoBadge = true,
                JsonFilePath = "DataModel/ControlInfoData.json",
                PathType = PathType.Relative
            }
        }, rootFrame, controlsSearchBox);
    }

    private void tabViewItem3_Loaded(object sender, RoutedEventArgs e)
    {
       NavWithJson2 = new NavigationManager(NavigationViewControl2, new NavigationViewOptions
        {
            DefaultPage = typeof(HomeLandingsPage),
            SettingsPage = typeof(GeneralPage),
            JsonOptions = new JsonOptions
            {
                HasInfoBadge = true,
                JsonFilePath = "DataModel/ControlInfoData.json",
                PathType = PathType.Relative,
                ItemPage = typeof(ItemPage),
                SectionPage = typeof(SectionPage)
            }
        }, rootFrame2, controlsSearchBox2);
    }
}
