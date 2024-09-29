using System.ComponentModel;
using System.Runtime.CompilerServices;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace WinUICommunity;
public abstract partial class ItemsPageBase : Page, INotifyPropertyChanged
{
    public event EventHandler<RoutedEventArgs> OnItemClick;

    public event PropertyChangedEventHandler PropertyChanged;

    private string _itemId;
    private IEnumerable<DataItem> _items;

    public IEnumerable<DataItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    /// <summary>
    /// Gets a value indicating whether the application's view is currently in "narrow" mode - i.e. on a mobile-ish device.
    /// </summary>
    protected virtual bool GetIsNarrowLayoutState()
    {
        throw new NotImplementedException();
    }

    protected void OnItemGridViewContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (sender.ContainerFromItem(sender.Items.LastOrDefault()) is GridViewItem container)
        {
            container.XYFocusDown = container;
        }

        var item = args.Item as DataItem;
        if (item != null)
        {
            args.ItemContainer.IsEnabled = item.IncludedInBuild;
            args.ItemContainer.Visibility = item.HideItem ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    protected void OnItemGridViewItemClick(object sender, ItemClickEventArgs e)
    {
        var gridView = (GridView)sender;
        var item = (DataItem)e.ClickedItem;

        _itemId = item.UniqueId;

        if (OnItemClick == null)
        {
            AllLandingPage.Instance.Navigate(sender, e);
        }

        OnItemClick?.Invoke(gridView, e);
    }

    protected void OnItemGridViewLoaded(object sender, RoutedEventArgs e)
    {
        if (_itemId != null)
        {
            var gridView = (GridView)sender;
            var items = gridView.ItemsSource as IEnumerable<DataItem>;
            var item = items?.FirstOrDefault(s => s.UniqueId == _itemId);
            if (item != null)
            {
                gridView.ScrollIntoView(item);
                ((GridViewItem)gridView.ContainerFromItem(item))?.Focus(FocusState.Programmatic);
            }
        }
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(storage, value)) return false;

        storage = value;
        NotifyPropertyChanged(propertyName);
        return true;
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public IJsonNavigationViewService JsonNavigationViewService
    {
        get { return (IJsonNavigationViewService)GetValue(JsonNavigationViewServiceProperty); }
        set { SetValue(JsonNavigationViewServiceProperty, value); }
    }

    public ImageSource PlaceholderSource
    {
        get => (ImageSource)GetValue(PlaceholderSourceProperty);
        set => SetValue(PlaceholderSourceProperty, value);
    }
    public bool IsCacheEnabled
    {
        get => (bool)GetValue(IsCacheEnabledProperty);
        set => SetValue(IsCacheEnabledProperty, value);
    }
    public bool EnableLazyLoading
    {
        get => (bool)GetValue(EnableLazyLoadingProperty);
        set => SetValue(EnableLazyLoadingProperty, value);
    }
    public double LazyLoadingThreshold
    {
        get => (double)GetValue(LazyLoadingThresholdProperty);
        set => SetValue(LazyLoadingThresholdProperty, value);
    }

    public string HeaderImage
    {
        get => (string)GetValue(HeaderImageProperty);
        set => SetValue(HeaderImageProperty, value);
    }
    public string HeaderOverlayImage
    {
        get => (string)GetValue(HeaderOverlayImageProperty);
        set => SetValue(HeaderOverlayImageProperty, value);
    }
    public static readonly DependencyProperty LazyLoadingThresholdProperty = DependencyProperty.Register(nameof(LazyLoadingThreshold), typeof(double), typeof(ItemsPageBase), new PropertyMetadata(300.0));
    public static readonly DependencyProperty EnableLazyLoadingProperty = DependencyProperty.Register(nameof(EnableLazyLoading), typeof(bool), typeof(ItemsPageBase), new PropertyMetadata(true));
    public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register(nameof(PlaceholderSource), typeof(ImageSource), typeof(ItemsPageBase), new PropertyMetadata(default(ImageSource)));
    public static readonly DependencyProperty IsCacheEnabledProperty = DependencyProperty.Register(nameof(IsCacheEnabled), typeof(bool), typeof(ItemsPageBase), new PropertyMetadata(true));
    public static readonly DependencyProperty HeaderImageProperty = DependencyProperty.Register(nameof(HeaderImage), typeof(string), typeof(ItemsPageBase), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty HeaderOverlayImageProperty = DependencyProperty.Register(nameof(HeaderOverlayImage), typeof(string), typeof(ItemsPageBase), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty JsonNavigationViewServiceProperty = DependencyProperty.Register(nameof(JsonNavigationViewService), typeof(IJsonNavigationViewService), typeof(ItemsPageBase), new PropertyMetadata(null));

    public bool CanExecuteInternalCommand { get; set; } = true;
}
