using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media.Animation;

namespace WinUICommunity;
public partial class JsonNavigationViewService
{
    private bool _useBreadcrumbBar;
    private bool _disableNavigationViewNavigator;
    private bool _allowDuplication;

    private BreadcrumbNavigator _mainBreadcrumb { get; set; }
    private ObservableCollection<NavigationBreadcrumb> BreadCrumbs { get; set; }
    private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (args.Index < BreadCrumbs.Count - 1)
        {
            var crumb = (NavigationBreadcrumb)args.Item;
            NavigateFromBreadcrumb(crumb.Page, args.Index);
        }
    }
    private void UpdateBreadcrumb()
    {
        _mainBreadcrumb.ItemsSource = BreadCrumbs;
        if (BreadCrumbs != null && BreadCrumbs?.Count > 0)
        {
            _navigationView.AlwaysShowHeader = true;
        }
        else
        {
            _navigationView.AlwaysShowHeader = false;
        }
    }
    public void NavigateFromBreadcrumb(Type TargetPageType, int BreadcrumbBarIndex, bool NavigatingBackwardsFromBreadcrumb = true)
    {
        SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
        info.Effect = SlideNavigationTransitionEffect.FromLeft;
        NavigateTo(TargetPageType, null, false, info);

        int indexToRemoveAfter = BreadcrumbBarIndex;

        if (indexToRemoveAfter < BreadCrumbs.Count - 1)
        {
            int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter - 1;

            for (int i = 0; i < itemsToRemove; i++)
            {
                BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
            }
        }
    }
}
