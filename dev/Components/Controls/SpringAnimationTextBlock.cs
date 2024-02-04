using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;

namespace WinUICommunity;

[TemplatePart(Name = nameof(PART_FocusBottomVisual), Type = typeof(Visual))]
[TemplatePart(Name = nameof(PART_FocusTopVisual), Type = typeof(Visual))]
[TemplatePart(Name = nameof(PART_RelaxBottomVisual), Type = typeof(Visual))]
[TemplatePart(Name = nameof(PART_RelaxTopVisual), Type = typeof(Visual))]
[TemplatePart(Name = nameof(PART_RootElement), Type = typeof(FrameworkElement))]
public class SpringAnimationTextBlock : Control
{
    private string PART_FocusBottomVisual = "PART_FocusBottomVisual";
    private string PART_FocusTopVisual = "PART_FocusTopVisual";
    private string PART_RelaxBottomVisual = "PART_RelaxBottomVisual";
    private string PART_RelaxTopVisual = "PART_RelaxTopVisual";
    private string PART_RootElement = "PART_RootElement";

    private Visual _focusBottomVisual;
    private Visual _focusTopVisual;
    private Visual _relaxBottomVisual;
    private Visual _relaxTopVisual;
    private FrameworkElement _rootElement;

    private Compositor _compositor;

    public TextBlock TopTextBlock
    {
        get { return (TextBlock)GetValue(TopTextBlockProperty); }
        set { SetValue(TopTextBlockProperty, value); }
    }

    public TextBlock BottomTextBlock
    {
        get { return (TextBlock)GetValue(BottomTextBlockProperty); }
        set { SetValue(BottomTextBlockProperty, value); }
    }

    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public double TextRotation
    {
        get { return (double)GetValue(TextRotationProperty); }
        set { SetValue(TextRotationProperty, value); }
    }

    public static readonly DependencyProperty TopTextBlockProperty =
        DependencyProperty.Register(nameof(TopTextBlock), typeof(TextBlock), typeof(SpringAnimationTextBlock), new PropertyMetadata(default(TextBlock)));

    public static readonly DependencyProperty BottomTextBlockProperty =
        DependencyProperty.Register(nameof(BottomTextBlock), typeof(TextBlock), typeof(SpringAnimationTextBlock), new PropertyMetadata(default(TextBlock)));

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(SpringAnimationTextBlock), new PropertyMetadata(false, OnIsActiveChanged));

    public static readonly DependencyProperty TextRotationProperty =
        DependencyProperty.Register(nameof(TextRotation), typeof(double), typeof(SpringAnimationTextBlock), new PropertyMetadata(-8.0));

    private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (SpringAnimationTextBlock)d;
        if (ctl != null)
        {
            if ((bool)e.NewValue)
            {
                ctl.UpdateOffsetUsingAnimation(true);
            }
            else
            {
                ctl.UpdateOffsetUsingAnimation(false);
            } 
        }
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

        var focusBottomVisual = GetTemplateChild(PART_FocusBottomVisual) as UIElement;
        var focusTopVisual = GetTemplateChild(PART_FocusTopVisual) as UIElement;
        var relaxBottomVisual = GetTemplateChild(PART_RelaxBottomVisual) as UIElement;
        var relaxTopVisual = GetTemplateChild(PART_RelaxTopVisual) as UIElement;

        _rootElement = GetTemplateChild(PART_RootElement) as FrameworkElement;

        _focusTopVisual = ElementCompositionPreview.GetElementVisual(focusTopVisual);
        _focusBottomVisual = ElementCompositionPreview.GetElementVisual(focusBottomVisual);
        _relaxTopVisual = ElementCompositionPreview.GetElementVisual(relaxTopVisual);
        _relaxBottomVisual = ElementCompositionPreview.GetElementVisual(relaxBottomVisual);

        SizeChanged += (s, e) =>
        {
            UpdateOffsetUsingAnimation(false);
        };

        UpdateOffsetUsingAnimation(false);

        UpdateOffset(false);
    }

    public void StartOffsetAnimation(Visual visual, float offsetX)
    {
        var springAnimation = _compositor.CreateSpringVector3Animation();
        springAnimation.DampingRatio = 0.85f;
        springAnimation.Period = TimeSpan.FromMilliseconds(50);
        springAnimation.FinalValue = new Vector3(offsetX, 0, 0);
        visual.StartAnimation(nameof(visual.Offset), springAnimation);
    }

    public void UpdateOffset(bool isActive)
    {
        var width = (float)_rootElement.Width;
        if (isActive)
        {
            _focusTopVisual.Offset = new Vector3(0, 0, 0);
            _focusBottomVisual.Offset = new Vector3(0, 0, 0);
            _relaxTopVisual.Offset = new Vector3(0, -2 * width, 0);
            _relaxBottomVisual.Offset = new Vector3(0, 2 * width, 0);
        }
        else
        {
            _focusTopVisual.Offset = new Vector3(0, 2 * width, 0);
            _focusBottomVisual.Offset = new Vector3(0, -2 * width, 0);
            _relaxTopVisual.Offset = new Vector3(0, 0, 0);
            _relaxBottomVisual.Offset = new Vector3(0, 0, 0);
        }
    }

    public void UpdateOffsetUsingAnimation(bool isActive)
    {
        var width = (float)_rootElement.Width;
        if (isActive)
        {
            StartOffsetAnimation(_focusTopVisual, 0);
            StartOffsetAnimation(_focusBottomVisual, 0);
            StartOffsetAnimation(_relaxTopVisual, -2 * width);
            StartOffsetAnimation(_relaxBottomVisual, 2 * width);
        }
        else
        {
            StartOffsetAnimation(_focusTopVisual, 2 * width);
            StartOffsetAnimation(_focusBottomVisual, -2 * width);
            StartOffsetAnimation(_relaxTopVisual, 0);
            StartOffsetAnimation(_relaxBottomVisual, 0);
        }
    }
}
