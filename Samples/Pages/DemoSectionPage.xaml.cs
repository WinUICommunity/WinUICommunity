using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class DemoSectionPage : Page
{
    public DemoSectionPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var item = NavigationServiceHelper.GetUniqueIdAndSectionId(e.Parameter);
        sectionPage.GetData(App.Current.JsonNavigationViewService.DataSource, item.UniqueId, item.SectionId);
        sectionPage.OrderBy(i => i.Title);
    }

    private void SectionPage_OnItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.JsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
