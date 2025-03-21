﻿namespace WucGalleryApp.Views;

public sealed partial class DemoSectionPage : Page
{
    public DemoSectionPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var item = AppHelper.GetUniqueIdAndSectionId(e.Parameter);
        sectionPage.GetData(App.Current.GetJsonNavigationViewService.DataSource, item.UniqueId, item.SectionId);
        sectionPage.OrderBy(i => i.Title);
    }
    private void SectionPage_OnItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var args = (ItemClickEventArgs)e;
        var item = (DataItem)args.ClickedItem;
        App.Current.GetJsonNavigationViewService.NavigateTo(item.UniqueId);
    }
}
