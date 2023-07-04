using System.Numerics;

using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using Windows.System;

namespace WinUICommunity;
public sealed partial class HeaderTile : UserControl
{
    public event EventHandler<RoutedEventArgs> OnItemClick;

    Compositor _compositor = Microsoft.UI.Xaml.Media.CompositionTarget.GetCompositorForCurrentThread();
    private SpringVector3NaturalMotionAnimation _springAnimation;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public object Source
    {
        get => (object)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    public string Link
    {
        get => (string)GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(HeaderTile), new PropertyMetadata(null));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(HeaderTile), new PropertyMetadata(null));

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register("Source", typeof(object), typeof(HeaderTile), new PropertyMetadata(null));

    public static readonly DependencyProperty LinkProperty =
        DependencyProperty.Register("Link", typeof(string), typeof(HeaderTile), new PropertyMetadata(null));

    public HeaderTile()
    {
        this.InitializeComponent();
    }

    private void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        CreateOrUpdateSpringAnimation(1.1f);
        (sender as UIElement).CenterPoint = new Vector3(70, 40, 1f);
        (sender as UIElement).StartAnimation(_springAnimation);
    }

    private void Element_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        CreateOrUpdateSpringAnimation(1.0f);
        (sender as UIElement).CenterPoint = new Vector3(70, 40, 1f);
        (sender as UIElement).StartAnimation(_springAnimation);
    }

    private void CreateOrUpdateSpringAnimation(float finalValue)
    {
        if (_springAnimation == null)
        {
            if (_compositor != null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }
        }

        _springAnimation.FinalValue = new Vector3(finalValue);
    }

    private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        OnItemClick?.Invoke(sender, e);
        if (Link != null)
        {
            await Launcher.LaunchUriAsync(new Uri(Link));
        }
    }
}

