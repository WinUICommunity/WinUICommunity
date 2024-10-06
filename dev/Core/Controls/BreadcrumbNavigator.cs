using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace WinUICommunity;
public sealed partial class BreadcrumbNavigator : BreadcrumbBar
{
    private bool userHasItemClickEvent = false;

    public new event TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs> ItemClicked
    {
        add
        {
            // If a handler other than your own is added, set the flag
            if (value != (TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs>)OnItemClicked)
            {
                userHasItemClickEvent = true;
            }
            base.ItemClicked += value;
        }
        remove
        {
            // If a handler other than your own is removed, check the flag
            if (value != (TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs>)OnItemClicked)
            {
                userHasItemClickEvent = false;
            }
            base.ItemClicked -= value;
        }
    }
    public ObservableCollection<NavigationBreadcrumb> BreadCrumbs
    {
        get { return (ObservableCollection<NavigationBreadcrumb>)GetValue(BreadCrumbsProperty); }
        set { SetValue(BreadCrumbsProperty, value); }
    }

    public static readonly DependencyProperty BreadCrumbsProperty =
        DependencyProperty.Register(nameof(BreadCrumbs), typeof(ObservableCollection<NavigationBreadcrumb>), typeof(BreadcrumbNavigator), new PropertyMetadata(null, OnBreadCrumbsChanged));

    private static void OnBreadCrumbsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (BreadcrumbNavigator)d;
        if (ctl != null)
        {
            ctl.ItemsSource = e.NewValue;
        }
    }

    public bool UseBuiltInEventForFrame
    {
        get { return (bool)GetValue(UseBuiltInEventForFrameProperty); }
        set { SetValue(UseBuiltInEventForFrameProperty, value); }
    }

    public static readonly DependencyProperty UseBuiltInEventForFrameProperty =
        DependencyProperty.Register(nameof(UseBuiltInEventForFrame), typeof(bool), typeof(BreadcrumbNavigator), new PropertyMetadata(false));

    public Frame Frame
    {
        get { return (Frame)GetValue(FrameProperty); }
        set { SetValue(FrameProperty, value); }
    }

    public static readonly DependencyProperty FrameProperty =
        DependencyProperty.Register(nameof(Frame), typeof(Frame), typeof(BreadcrumbNavigator), new PropertyMetadata(null));

    public BreadcrumbNavigator()
    {
        ItemClicked -= OnItemClicked;
        ItemClicked += OnItemClicked;

        if (Frame != null && UseBuiltInEventForFrame)
        {
            Frame.Navigating -= OnFrameNavigating;
            Frame.Navigating += OnFrameNavigating;
        }
    }

    private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        AddNewItem(e.SourcePageType, null);
    }

    private void OnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (!userHasItemClickEvent)
        {
            OnItemClicked(args);
        }
    }

    public void OnItemClicked(BreadcrumbBarItemClickedEventArgs args)
    {
        if (args.Index < BreadCrumbs.Count - 1)
        {
            var crumb = (NavigationBreadcrumb)args.Item;
            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            AddNewItem(crumb.Page, crumb.Parameter, info, null);
            FixIndex(args.Index);
        }
    }

    public void FixIndex(int BreadcrumbBarIndex)
    {
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

    internal void AddNewItem(NavigationView navigationView, Type targetPageType, NavigationTransitionInfo navigationTransitionInfo, object parameter, object currentPageParameter, bool allowDuplication, bool disableNavigationViewNavigator, Action updateBreadcrumb)
    {
        string pageTitle = string.Empty;
        bool isHeaderVisibile = true;
        bool clearNavigation = true;

        DependencyObject obj = Activator.CreateInstance(targetPageType) as DependencyObject;

        var pageTitleAttached = GetPageTitle(obj);
        isHeaderVisibile = GetIsHeaderVisible(obj);
        clearNavigation = GetClearNavigation(obj);

        if (!string.IsNullOrEmpty(pageTitleAttached))
        {
            pageTitle = pageTitleAttached;
        }
        else
        {
            if (currentPageParameter != null && currentPageParameter is string value)
            {
                pageTitle = value;
            }
            else if (!disableNavigationViewNavigator && currentPageParameter != null && currentPageParameter is DataItem dataItem)
            {
                pageTitle = dataItem.Title;
            }
            else if (!disableNavigationViewNavigator && currentPageParameter != null && currentPageParameter is DataGroup dataGroup)
            {
                pageTitle = dataGroup.Title;
            }
        }

        if (disableNavigationViewNavigator && currentPageParameter != null && (currentPageParameter is DataItem || currentPageParameter is DataGroup))
        {
            isHeaderVisibile = false;
        }
        else
        {
            if (Frame != null && clearNavigation)
            {
                BreadCrumbs.Clear();
                this.Frame.BackStack.Clear();
            }
        }

        if (isHeaderVisibile)
        {
            if (!string.IsNullOrEmpty(pageTitle))
            {
                if (BreadCrumbs != null)
                {
                    var currentItem = new NavigationBreadcrumb(pageTitle, targetPageType, parameter);

                    if (allowDuplication)
                    {
                        BreadCrumbs?.Add(currentItem);
                        updateBreadcrumb?.Invoke();
                    }
                    else
                    {
                        var itemExist = BreadCrumbs.Contains(currentItem, new GenericCompare<NavigationBreadcrumb>(x => x.Page));
                        if (!itemExist)
                        {
                            BreadCrumbs?.Add(currentItem);
                            updateBreadcrumb?.Invoke();
                        }
                    }
                }
            }
        }

        if (BreadCrumbs == null || BreadCrumbs?.Count == 0)
        {
            navigationView.AlwaysShowHeader = false;
            ChangeBreadcrumbVisibility(false);
        }
        else
        {
            if (navigationView != null)
            {
                navigationView.AlwaysShowHeader = isHeaderVisibile;
            }
            ChangeBreadcrumbVisibility(isHeaderVisibile);
        }

        if (this.Frame != null)
        {
            this.Frame.Navigate(targetPageType, parameter, navigationTransitionInfo);
        }
    }

    public void AddNewItem(Type targetPageType, object parameter, NavigationTransitionInfo navigationTransitionInfo, Action updateBreadcrumb)
    {
        AddNewItem(null, targetPageType, navigationTransitionInfo, parameter, null, true, false, updateBreadcrumb);
    }
    public void AddNewItem(Type targetPageType, Action updateBreadcrumb)
    {
        AddNewItem(null, targetPageType, null, null, null, true, false, updateBreadcrumb);
    }
    public void ChangeBreadcrumbVisibility(bool IsBreadcrumbVisible)
    {
        if (IsBreadcrumbVisible)
        {
            Visibility = Visibility.Visible;
        }
        else
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
