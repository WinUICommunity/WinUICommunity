using System.Collections.ObjectModel;

namespace WinUICommunity;
public sealed partial class ColorPalette : UserControl
{
    public CornerRadius RectangleCornerRadius
    {
        get { return (CornerRadius)GetValue(RectangleCornerRadiusProperty); }
        set { SetValue(RectangleCornerRadiusProperty, value); }
    }
    public ColorType Color
    {
        get { return (ColorType)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }
    public PaletteType Palette
    {
        get { return (PaletteType)GetValue(ColorPaleteTypeProperty); }
        set { SetValue(ColorPaleteTypeProperty, value); }
    }

    public bool ShowHeader
    {
        get { return (bool)GetValue(ShowHeaderProperty); }
        set { SetValue(ShowHeaderProperty, value); }
    }

    public double ItemWidth
    {
        get { return (double)GetValue(ItemWidthProperty); }
        set { SetValue(ItemWidthProperty, value); }
    }

    public static readonly DependencyProperty RectangleCornerRadiusProperty =
        DependencyProperty.Register(nameof(RectangleCornerRadius), typeof(CornerRadius), typeof(ColorPalette), new PropertyMetadata(new CornerRadius(0)));

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(ColorType), typeof(ColorType), new PropertyMetadata(ColorType.Light, OnColorChanged));

    public static readonly DependencyProperty ColorPaleteTypeProperty =
        DependencyProperty.Register(nameof(Palette), typeof(PaletteType), typeof(ColorPalette), new PropertyMetadata(PaletteType.TabView, OnPaletteChanged));

    public static readonly DependencyProperty ShowHeaderProperty =
        DependencyProperty.Register(nameof(ShowHeader), typeof(bool), typeof(ColorPalette), new PropertyMetadata(true));

    public static readonly DependencyProperty ItemWidthProperty =
        DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(ColorPalette), new PropertyMetadata(120.0));


    public event SelectionChangedEventHandler SelectionChanged;
    public event ItemClickEventHandler ItemClick;

    public object ItemsSource
    {
        get => gridView.ItemsSource;
        set => gridView.ItemsSource = value;
    }

    public object SelectedItem
    {
        get => gridView.SelectedItem;
        set => gridView.SelectedItem = value;
    }
    public int SelectedIndex
    {
        get => gridView.SelectedIndex;
        set => gridView.SelectedIndex = value;
    }

    public DataTemplate ItemTemplate
    {
        get => gridView.ItemTemplate;
        set => gridView.ItemTemplate = value;
    }

    
    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ColorPalette)d;
        if (ctl != null)
        {
            ctl.LoadColorPallete();
        }
    }

    private void LoadColorPallete()
    {
        switch (Color)
        {
            case ColorType.Normal:
                gridView.ItemsSource = ColorPaletteNormalResources();
                break;
            case ColorType.Light:
                gridView.ItemsSource = ColorPaletteLightResources;
                break;
            case ColorType.Dark:
                gridView.ItemsSource = ColorPaletteDarkResources;
                break;
            case ColorType.LightAndDark:
                gridView.ItemsSource = ColorPaletteDarkAndLightResources();
                break;
            case ColorType.LightAndNormal:
                gridView.ItemsSource = ColorPaletteLightAndNormalResources();
                break;
            case ColorType.DarkAndNormal:
                gridView.ItemsSource = ColorPaletteDarkAndNormalResources();
                break;
            case ColorType.LightAndDarkAndNormal:
                gridView.ItemsSource = ColorPaletteLightAndDarkAndNormalResources();
                break;
        }
    }
    private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ColorPalette)d;
        if (ctl != null)
        {
            ctl.ChangeItemTemplate();
        }
    }

    private void ChangeItemTemplate()
    {
        DataTemplate dataTemplate = null;
        switch (Palette)
        {
            case PaletteType.TabView:
                dataTemplate = Resources["ColorPaletteTabViewTemplate"] as DataTemplate;
                break;
            case PaletteType.Circle:
                dataTemplate = Resources["ColorPaletteCircleTemplate"] as DataTemplate;
                break;
            case PaletteType.Rectangle:
                dataTemplate = Resources["ColorPaletteRectangleTemplate"] as DataTemplate;
                break;
        }
        gridView.ItemTemplate = dataTemplate;
    }
    
    private ObservableCollection<ColorPaletteItem> ColorPaletteLightAndDarkAndNormalResources()
    {
        var list = new ObservableCollection<ColorPaletteItem>();
        for (int i = 0; i < ColorPaletteLightResources.Count; i++)
        {
            list.Add(ColorPaletteLightResources[i]);
            list.Add(ColorPaletteDarkResources[i]);
            list.Add(ColorPaletteNormalResources()[i]);
        }
        return list;
    }
    private ObservableCollection<ColorPaletteItem> ColorPaletteDarkAndNormalResources()
    {
        var list = new ObservableCollection<ColorPaletteItem>();
        for (int i = 0; i < ColorPaletteDarkResources.Count; i++)
        {
            list.Add(ColorPaletteDarkResources[i]);
            list.Add(ColorPaletteNormalResources()[i]);
        }
        return list;
    }
    private ObservableCollection<ColorPaletteItem> ColorPaletteLightAndNormalResources()
    {
        var list = new ObservableCollection<ColorPaletteItem>();
        for (int i = 0; i < ColorPaletteLightResources.Count; i++)
        {
            list.Add(ColorPaletteLightResources[i]);
            list.Add(ColorPaletteNormalResources()[i]);
        }
        return list;
    }
    public ObservableCollection<ColorPaletteItem> ColorPaletteDarkAndLightResources()
    {
        var list = new ObservableCollection<ColorPaletteItem>();
        for (int i = 0; i < ColorPaletteDarkResources.Count; i++)
        {
            list.Add(ColorPaletteDarkResources[i]);
            list.Add(ColorPaletteLightResources[i]);
        }
        return list;
    }
    public ObservableCollection<ColorPaletteItem> ColorPaletteNormalResources()
    {
        var list = new ObservableCollection<ColorPaletteItem>();
        for (int i = 0; i < ColorPaletteDarkResources.Count; i++)
        {
            list.Add(new ColorPaletteItem
            {
                Name = ColorPaletteDarkResources[i].Name,
                Hex = ColorPaletteDarkResources[i].ActualHex,
                ActualHex = ColorPaletteDarkResources[i].ActualHex,
            });
        }
        return list;
    }
    public ObservableCollection<ColorPaletteItem> ColorPaletteLightResources { get; } = new ObservableCollection<ColorPaletteItem>()
        {
            new ColorPaletteItem
            {
                ActualHex = "#000000", /* Transparent */
                Hex = "#00000000", /* Transparent */
                Name = "Transparent"
            },
            new ColorPaletteItem
            {
                ActualHex = "#f44336", /* #f44336 */
                Hex = "#32f44336", /* #f44336 */
                Name = "Red"
            },
            new ColorPaletteItem
            {
                ActualHex = "#e91e63", /* #e91e63 */
                Hex = "#32e91e63", /* #e91e63 */
                Name = "Pink"
            },
            new ColorPaletteItem
            {
                ActualHex = "#673ab7", /* #673ab7 */
                Hex = "#32673ab7", /* #673ab7 */
                Name = "Purple"
            },
            new ColorPaletteItem
            {
                ActualHex = "#3f51b5", /* #3f51b5 */
                Hex = "#323f51b5", /* #3f51b5 */
                Name = "Indigo"
            },
            new ColorPaletteItem
            {
                ActualHex = "#2196f3", /* #2196f3 */
                Hex = "#322196f3", /* #2196f3 */
                Name = "Blue"
            },
            new ColorPaletteItem
            {
                ActualHex = "#03a9f4", /* #03a9f4 */
                Hex = "#3203a9f4", /* #03a9f4 */
                Name = "Light Blue"
            },
            new ColorPaletteItem
            {
                ActualHex = "#00bcd4", /* #00bcd4 */
                Hex = "#3200bcd4", /* #00bcd4 */
                Name = "Cyan"
            },
            new ColorPaletteItem
            {
                ActualHex = "#009688", /* #009688 */
                Hex = "#32009688", /* #009688 */
                Name = "Teal"
            },
            new ColorPaletteItem
            {
                ActualHex = "#4caf50", /* #4caf50 */
                Hex = "#324caf50", /* #4caf50 */
                Name = "Green"
            },
            new ColorPaletteItem
            {
                ActualHex = "#8bc34a", /* #8bc34a */
                Hex = "#328bc34a", /* #8bc34a */
                Name = "Light Green"
            },
            new ColorPaletteItem
            {
                ActualHex = "#cddc39", /* #cddc39 */
                Hex = "#32cddc39", /* #cddc39 */
                Name = "Lime"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ffeb3b", /* #ffeb3b */
                Hex = "#32ffeb3b", /* #ffeb3b */
                Name = "Yellow"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ffc107", /* #ffc107 */
                Hex = "#32ffc107", /* #ffc107 */
                Name = "Amber"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ff9800", /* #ff9800 */
                Hex = "#32ff9800", /* #ff9800 */
                Name = "Orange"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ff5722", /* #ff5722 */
                Hex = "#32ff5722", /* #ff5722 */
                Name = "Deep Orange"
            },
            new ColorPaletteItem
            {
                ActualHex = "#795548", /* #795548 */
                Hex = "#32795548", /* #795548 */
                Name = "Brown"
            },
            new ColorPaletteItem
            {
                ActualHex = "#9e9e9e", /* #9e9e9e */
                Hex = "#329e9e9e", /* #9e9e9e */
                Name = "Grey"
            }
        };
    public ObservableCollection<ColorPaletteItem> ColorPaletteDarkResources { get; } = new ObservableCollection<ColorPaletteItem>()
        {
            new ColorPaletteItem
            {
                ActualHex = "#000000", /* Transparent */
                Hex = "#00000000", /* Transparent */
                Name = "Transparent"
            },
            new ColorPaletteItem
            {
                ActualHex = "#f44336", /* #f44336 */
                Hex = "#64f44336", /* #f44336 */
                Name = "Dark Red"
            },
            new ColorPaletteItem
            {
                ActualHex = "#e91e63", /* #e91e63 */
                Hex = "#64e91e63", /* #e91e63 */
                Name = "Dark Pink"
            },
            new ColorPaletteItem
            {
                ActualHex = "#673ab7", /* #673ab7 */
                Hex = "#64673ab7", /* #673ab7 */
                Name = "Dark Purple"
            },
            new ColorPaletteItem
            {
                ActualHex = "#3f51b5", /* #3f51b5 */
                Hex = "#643f51b5", /* #3f51b5 */
                Name = "Dark Indigo"
            },
            new ColorPaletteItem
            {
                ActualHex = "#2196f3", /* #2196f3 */
                Hex = "#642196f3", /* #2196f3 */
                Name = "Dark Blue"
            },
            new ColorPaletteItem
            {
                ActualHex = "#03a9f4", /* #03a9f4 */
                Hex = "#6403a9f4", /* #03a9f4 */
                Name = "Dark Light Blue"
            },
            new ColorPaletteItem
            {
                ActualHex = "#00bcd4", /* #00bcd4 */
                Hex = "#6400bcd4", /* #00bcd4 */
                Name = "Dark Cyan"
            },
            new ColorPaletteItem
            {
                ActualHex = "#009688", /* #009688 */
                Hex = "#64009688", /* #009688 */
                Name = "Dark Teal"
            },
            new ColorPaletteItem
            {
                ActualHex = "#4caf50", /* #4caf50 */
                Hex = "#644caf50", /* #4caf50 */
                Name = "Dark Green"
            },
            new ColorPaletteItem
            {
                ActualHex = "#8bc34a", /* #8bc34a */
                Hex = "#648bc34a", /* #8bc34a */
                Name = "Dark Light Green"
            },
            new ColorPaletteItem
            {
                ActualHex = "#cddc39", /* #cddc39 */
                Hex = "#64cddc39", /* #cddc39 */
                Name = "Dark Lime"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ffeb3b", /* #ffeb3b */
                Hex = "#64ffeb3b", /* #ffeb3b */
                Name = "Dark Yellow"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ffc107", /* #ffc107 */
                Hex = "#64ffc107", /* #ffc107 */
                Name = "Dark Amber"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ff9800", /* #ff9800 */
                Hex = "#64ff9800", /* #ff9800 */
                Name = "Dark Orange"
            },
            new ColorPaletteItem
            {
                ActualHex = "#ff5722", /* #ff5722 */
                Hex = "#64ff5722", /* #ff5722 */
                Name = "Dark Deep Orange"
            },
            new ColorPaletteItem
            {
                ActualHex = "#795548", /* #795548 */
                Hex = "#64795548", /* #795548 */
                Name = "Dark Brown"
            },
            new ColorPaletteItem
            {
                ActualHex = "#9e9e9e", /* #9e9e9e */
                Hex = "#649e9e9e", /* #9e9e9e */
                Name = "Dark Grey"
            }
        };
    public ColorPalette()
    {
        this.InitializeComponent();
        DataContext = this;
        Color = ColorType.Light;
        if (ItemsSource != null)
        {
            gridView.ItemsSource = ItemsSource;
        }

        Palette = PaletteType.TabView;
    }

    private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectionChanged?.Invoke(sender, e);
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ItemClick?.Invoke(sender, e);
    }
}

public enum PaletteType
{
    TabView,
    Circle,
    Rectangle
}

public enum ColorType
{
    Normal,
    Light,
    Dark,
    LightAndDark,
    LightAndNormal,
    DarkAndNormal,
    LightAndDarkAndNormal
}
