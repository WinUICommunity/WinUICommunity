// https://github.com/DinoChan

using Microsoft.UI.Xaml.Media;

namespace WinUICommunity;
public class BubbleButton : Button
{
    public static readonly DependencyProperty BubbleForegroundProperty =
        DependencyProperty.Register(nameof(BubbleForeground), typeof(Brush), typeof(BubbleButton), new PropertyMetadata(null));
    public Brush BubbleForeground
    {
        get { return (Brush)GetValue(BubbleForegroundProperty); }
        set { SetValue(BubbleForegroundProperty, value); }
    }
}
