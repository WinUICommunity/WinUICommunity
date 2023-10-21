using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WinUICommunity;
public partial class SettingsGroup : Control
{
    public ObservableCollection<object> Items
    {
        get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
        set { SetValue(ItemsProperty, value); }
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(SettingsGroup), new PropertyMetadata(null, OnItemsConnectedPropertyChanged));

    private static void OnItemsConnectedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is SettingsGroup settingGroup)
        {
            settingGroup.OnItemsChanged();
        }
    }

    private void OnItemsChanged()
    {
        if (_itemsRepeater is not null)
        {
            Bool2CornerRadiusConverter bool2CornerRadiusConverter = new Bool2CornerRadiusConverter();
            var cornerRadius = bool2CornerRadiusConverter.Convert(Items.Count, null, null, null);

            if (_rootGrid != null)
            {
                _rootGrid.CornerRadius = (CornerRadius)cornerRadius;

            }

            foreach (SettingsCard item in Items)
            {
                if (item == null)
                {
                    continue;
                }
                item.Padding = new Thickness(30, 0, 25, 0);
                item.CornerRadius = new CornerRadius(0);
                item.BorderThickness = new Thickness(1,1,1,0);

                SettingsCard firstItem = (SettingsCard)Items.FirstOrDefault();
                if (firstItem != null)
                {
                    firstItem.BorderThickness = new Thickness(1,0,1,0);
                }
                SettingsCard lastItem = (SettingsCard)Items.LastOrDefault();
                if (lastItem != null && Items.Count == 1)
                {
                    lastItem.CornerRadius = new CornerRadius(0, 0, 4, 4);
                    lastItem.BorderThickness = new Thickness(1,0,1,1);
                }
                else if (lastItem != null)
                {
                    lastItem.CornerRadius = new CornerRadius(0, 0, 4, 4);
                    lastItem.BorderThickness = new Thickness(1);
                }
            }

            _itemsRepeater.ItemsSource = Items;
        }
    }

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(SettingsGroup), new PropertyMetadata(default(object), OnContentPropertyChanged));

    public object Content
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (SettingsGroup)d;
        if (ctl == null)
        {
            return;
        }
        ctl.OnContentChanged();
    }

    private void OnContentChanged()
    {
        if (_contentPresenter != null)
        {
            _contentPresenter.Visibility = Content == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public static readonly DependencyProperty HeaderIconProperty =
        DependencyProperty.Register(nameof(HeaderIcon), typeof(IconElement), typeof(SettingsGroup), new PropertyMetadata(defaultValue: null, (d, e) => ((SettingsGroup)d).OnHeaderIconPropertyChanged((IconElement)e.OldValue, (IconElement)e.NewValue)));

    public IconElement HeaderIcon
    {
        get => (IconElement)GetValue(HeaderIconProperty);
        set => SetValue(HeaderIconProperty, value);
    }

    protected virtual void OnHeaderIconPropertyChanged(IconElement oldValue, IconElement newValue)
    {
        OnHeaderIconChanged();
    }

    private void OnHeaderIconChanged()
    {
        if (GetTemplateChild(PartHeaderIconPresenter) is FrameworkElement headerIconPresenter)
        {
            headerIconPresenter.Visibility = HeaderIcon != null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(Header), typeof(object), typeof(SettingsGroup), new PropertyMetadata(null, OnHeaderPropertyChanged));

    [Localizable(true)]
    public object Header
    {
        get => (object)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (SettingsGroup)d;
        if (ctl == null)
        {
            return;
        }
        ctl.OnHeaderChanged();
    }

    private void OnHeaderChanged()
    {
        if (_headerPresenter != null)
        {
            _headerPresenter.Visibility = Header == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(object), typeof(SettingsGroup), new PropertyMetadata(null, OnDescriptionPropertyChanged));

    [Localizable(true)]
    public object Description
    {
        get => (object)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    private static void OnDescriptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (SettingsGroup)d;

        if (ctl == null)
        {
            return;
        }
        ctl.OnDescriptionChanged();
    }

    private void OnDescriptionChanged()
    {
        if (_descriptionPresenter != null)
        {
            _descriptionPresenter.Visibility = Description == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
