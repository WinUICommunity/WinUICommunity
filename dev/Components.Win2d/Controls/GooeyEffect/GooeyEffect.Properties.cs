namespace WinUICommunity;
public partial class GooeyEffect
{
    public static readonly DependencyProperty SecondTimeLineFromXProperty =
        DependencyProperty.Register(nameof(SecondTimeLineFromX), typeof(double), typeof(GooeyEffect), new PropertyMetadata(-100.0, OnAnimationChanged));
    public double SecondTimeLineFromX
    {
        get { return (double)GetValue(SecondTimeLineFromXProperty); }
        set { SetValue(SecondTimeLineFromXProperty, value); }
    }

    public static readonly DependencyProperty SecondTimeLineToXProperty =
        DependencyProperty.Register(nameof(SecondTimeLineToX), typeof(double), typeof(GooeyEffect), new PropertyMetadata(100.0, OnAnimationChanged));
    public double SecondTimeLineToX
    {
        get { return (double)GetValue(SecondTimeLineToXProperty); }
        set { SetValue(SecondTimeLineToXProperty, value); }
    }

    public static readonly DependencyProperty FirstTimeLineFromXProperty =
        DependencyProperty.Register(nameof(FirstTimeLineFromX), typeof(double), typeof(GooeyEffect), new PropertyMetadata(100.0, OnAnimationChanged));
    public double FirstTimeLineFromX
    {
        get { return (double)GetValue(FirstTimeLineFromXProperty); }
        set { SetValue(FirstTimeLineFromXProperty, value); }
    }

    public static readonly DependencyProperty FirstTimeLineToXProperty =
        DependencyProperty.Register(nameof(FirstTimeLineToX), typeof(double), typeof(GooeyEffect), new PropertyMetadata(-100.0, OnAnimationChanged));
    public double FirstTimeLineToX
    {
        get { return (double)GetValue(FirstTimeLineToXProperty); }
        set { SetValue(FirstTimeLineToXProperty, value); }
    }

    public static readonly DependencyProperty SecondTimeLineFromYProperty =
        DependencyProperty.Register(nameof(SecondTimeLineFromY), typeof(double), typeof(GooeyEffect), new PropertyMetadata(0.0, OnAnimationChanged));
    public double SecondTimeLineFromY
    {
        get { return (double)GetValue(SecondTimeLineFromYProperty); }
        set { SetValue(SecondTimeLineFromYProperty, value); }
    }

    public static readonly DependencyProperty SecondTimeLineToYProperty =
        DependencyProperty.Register(nameof(SecondTimeLineToY), typeof(double), typeof(GooeyEffect), new PropertyMetadata(0.0, OnAnimationChanged));
    public double SecondTimeLineToY
    {
        get { return (double)GetValue(SecondTimeLineToYProperty); }
        set { SetValue(SecondTimeLineToYProperty, value); }
    }

    public static readonly DependencyProperty FirstTimeLineFromYProperty =
        DependencyProperty.Register(nameof(FirstTimeLineFromY), typeof(double), typeof(GooeyEffect), new PropertyMetadata(0.0, OnAnimationChanged));
    public double FirstTimeLineFromY
    {
        get { return (double)GetValue(FirstTimeLineFromYProperty); }
        set { SetValue(FirstTimeLineFromYProperty, value); }
    }

    public static readonly DependencyProperty FirstTimeLineToYProperty =
        DependencyProperty.Register(nameof(FirstTimeLineToY), typeof(double), typeof(GooeyEffect), new PropertyMetadata(0.0, OnAnimationChanged));
    public double FirstTimeLineToY
    {
        get { return (double)GetValue(FirstTimeLineToYProperty); }
        set { SetValue(FirstTimeLineToYProperty, value); }
    }

    public static readonly DependencyProperty DurationProperty =
        DependencyProperty.Register(nameof(Duration), typeof(double), typeof(GooeyEffect), new PropertyMetadata(2.0, OnAnimationChanged));
    public double Duration
    {
        get { return (double)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    public static readonly DependencyProperty EasingModeProperty =
        DependencyProperty.Register(nameof(EasingMode), typeof(EasingMode), typeof(GooeyEffect), new PropertyMetadata(EasingMode.EaseInOut, OnAnimationChanged));
    public EasingMode EasingMode
    {
        get { return (EasingMode)GetValue(EasingModeProperty); }
        set { SetValue(EasingModeProperty, value); }
    }

    public static readonly DependencyProperty SecondColorBrushProperty =
        DependencyProperty.Register(nameof(SecondColorBrush), typeof(Color), typeof(GooeyEffect), new PropertyMetadata(Colors.IndianRed, OnResourcePropertyValueChanged));
    public Color SecondColorBrush
    {
        get { return (Color)GetValue(SecondColorBrushProperty); }
        set { SetValue(SecondColorBrushProperty, value); }
    }

    public static readonly DependencyProperty FirstColorBrushProperty =
        DependencyProperty.Register(nameof(FirstColorBrush), typeof(Color), typeof(GooeyEffect), new PropertyMetadata(Colors.PaleVioletRed, OnResourcePropertyValueChanged));
    public Color FirstColorBrush
    {
        get { return (Color)GetValue(FirstColorBrushProperty); }
        set { SetValue(FirstColorBrushProperty, value); }
    }

    public static readonly DependencyProperty BlurAmountProperty =
        DependencyProperty.Register(nameof(BlurAmount), typeof(double), typeof(GooeyEffect), new PropertyMetadata(20.0, OnResourcePropertyValueChanged));
    public double BlurAmount
    {
        get { return (double)GetValue(BlurAmountProperty); }
        set { SetValue(BlurAmountProperty, value); }
    }

    public static readonly DependencyProperty SecondRadiusProperty =
        DependencyProperty.Register(nameof(SecondRadius), typeof(double), typeof(GooeyEffect), new PropertyMetadata(30.0, OnPropertyValueChanged));
    public double SecondRadius
    {
        get { return (double)GetValue(SecondRadiusProperty); }
        set { SetValue(SecondRadiusProperty, value); }
    }

    public static readonly DependencyProperty FirstRadiusProperty =
        DependencyProperty.Register(nameof(FirstRadius), typeof(double), typeof(GooeyEffect), new PropertyMetadata(30.0, OnPropertyValueChanged));

    public double FirstRadius
    {
        get { return (double)GetValue(FirstRadiusProperty); }
        set { SetValue(FirstRadiusProperty, value); }
    }
}
