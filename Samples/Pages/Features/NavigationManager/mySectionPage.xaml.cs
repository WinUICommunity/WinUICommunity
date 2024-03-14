using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;
public sealed partial class mySectionPage : Page
{
    public mySectionPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var item = NavigationServiceHelper.GetUniqueIdAndSectionId(e.Parameter);
        sectionPage.GetData(NavigationManagerPage.Instance.jsonNavigationViewService.DataSource, item.UniqueId, item.SectionId);
    }

    private void SectionPage_OnItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        NavigationManagerPage.Instance.jsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
