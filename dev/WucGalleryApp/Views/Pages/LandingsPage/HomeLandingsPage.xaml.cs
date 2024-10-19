namespace WucGalleryApp.Views;

public sealed partial class HomeLandingsPage : Page
{
    public HomeLandingsPage()
    {
        this.InitializeComponent();
    }

    private void mainLandingsPage_Loaded(object sender, RoutedEventArgs e)
    {
        mainLandingsPage.GetData(App.Current.GetJsonNavigationViewService.DataSource);
    }

    private void mainLandingsPage_OnItemClick(object sender, RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.GetJsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
