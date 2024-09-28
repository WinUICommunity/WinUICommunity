using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;

namespace WinUICommunity;

public sealed partial class BreadcrumbNavigator : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private bool CanExecuteInternalCommand { get; set; } = true;

    public event TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs> ItemClicked
    {
        add
        {
            CanExecuteInternalCommand = false;
            MainBreadcrumbBar.ItemClicked += value;
        }
        remove
        {
            CanExecuteInternalCommand = true;
            MainBreadcrumbBar.ItemClicked -= value;
        }
    }

    private IJsonNavigationViewService jsonNavigationViewService;
    public IJsonNavigationViewService JsonNavigationViewService
    {
        get { return jsonNavigationViewService; }
        set { jsonNavigationViewService = value; }
    }

    private ObservableCollection<string> breadcrumbBarCollection;
    public ObservableCollection<string> BreadcrumbBarCollection
    {
        get { return breadcrumbBarCollection; }
        set
        {
            breadcrumbBarCollection = value;
            OnPropertyChanged();
        }
    }

    public string PrimaryItemText
    {
        get { return (string)GetValue(PrimaryItemTextProperty); }
        set { SetValue(PrimaryItemTextProperty, value); }
    }

    public static readonly DependencyProperty PrimaryItemTextProperty =
        DependencyProperty.Register(nameof(PrimaryItemText), typeof(string), typeof(BreadcrumbNavigator), new PropertyMetadata(null, OnPrimaryItemTextChanged));

    private static void OnPrimaryItemTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (BreadcrumbNavigator)d;
        if (ctl != null)
        {
            if (e.NewValue != null && e.NewValue is string value)
            {
                ctl.Init(value);
            }
        }
    }

    public List<string> Items
    {
        get => (List<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(List<string>), typeof(BreadcrumbNavigator), new PropertyMetadata(null, OnItemsChanged));

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (BreadcrumbNavigator)d;
        if (ctl != null)
        {
            ctl.Init(ctl.PrimaryItemText);
        }
    }

    public string SecondaryItemText
    {
        get => (string)GetValue(SecondaryItemTextProperty);
        set => SetValue(SecondaryItemTextProperty, value);
    }

    public static readonly DependencyProperty SecondaryItemTextProperty =
        DependencyProperty.Register(nameof(SecondaryItemText), typeof(string), typeof(BreadcrumbNavigator), new PropertyMetadata(default(string), OnSecondaryItemTextChanged));

    private static void OnSecondaryItemTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (BreadcrumbNavigator)d;
        if (ctl != null)
        {
            ctl.Init(ctl.PrimaryItemText);
        }
    }

    public BreadcrumbNavigator()
    {
        this.InitializeComponent();
        BreadcrumbBarCollection = new ObservableCollection<string>();
        Loaded += BreadcrumbNavigator_Loaded;
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Parameter != null && e.Parameter is string value)
        {
            SecondaryItemText = value;
        }
    }

    private string GetPrimaryItemText()
    {
        if (JsonNavigationViewService != null)
        {
            var item = JsonNavigationViewService.SettingsItem as NavigationViewItem;
            if (item != null && item.Content != null)
            {
                return item.Content?.ToString();
            }
        }

        return "Settings";
    }
    private void BreadcrumbNavigator_Loaded(object sender, RoutedEventArgs e)
    {
        PrimaryItemText = GetPrimaryItemText();

        if (JsonNavigationViewService != null)
        {
            var secondaryItemText = JsonNavigationViewService.CurrentPageParameter;
            if (secondaryItemText != null && secondaryItemText is string value)
            {
                SecondaryItemText = value;
            }

            JsonNavigationViewService.Frame.Navigated -= Frame_Navigated;
            JsonNavigationViewService.Frame.Navigated += Frame_Navigated;
        }
    }

    private void Init(string primaryItemText)
    {
        BreadcrumbBarCollection?.Clear();
        BreadcrumbBarCollection.Add(primaryItemText);
        if (Items != null)
        {
            foreach (var item in Items)
            {
                BreadcrumbBarCollection.Add(item);
            }
        }
        else
        {
            BreadcrumbBarCollection.Add(SecondaryItemText);
        }
    }
    private void OnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (CanExecuteInternalCommand)
        {
            if (JsonNavigationViewService == null)
            {
                return;
            }

            int numItemsToGoBack = BreadcrumbBarCollection.Count - args.Index - 1;
            for (int i = 0; i < numItemsToGoBack; i++)
            {
                JsonNavigationViewService.GoBack();
            }
        }
    }
}

