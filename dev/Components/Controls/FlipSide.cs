using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using System.Numerics;

namespace WinUICommunity;
public sealed partial class FlipSide : Control
{
    public static readonly DependencyProperty IsFlippedProperty =
        DependencyProperty.Register("IsFlipped", typeof(bool), typeof(FlipSide), new PropertyMetadata(false, (s, a) =>
        {
            if (a.NewValue != a.OldValue)
            {
                if (s is FlipSide sender)
                {
                    sender.OnIsFlippedChanged();
                }
            }
        }));

    public static readonly DependencyProperty Side1Property =
        DependencyProperty.Register("Side1", typeof(object), typeof(FlipSide), new PropertyMetadata(null));

    public static readonly DependencyProperty Side2Property =
        DependencyProperty.Register("Side2", typeof(object), typeof(FlipSide), new PropertyMetadata(null));

    private Vector2 axis;

    private Grid LayoutRoot;

    private Visual s1Visual;

    private Visual s2Visual;

    private ContentPresenter Side1Content;

    private ContentPresenter Side2Content;

    private SpringScalarNaturalMotionAnimation springAnimation1;

    private SpringScalarNaturalMotionAnimation springAnimation2;

    public FlipSide()
    {
        axis = new Vector2(0, 1);
        this.DefaultStyleKey = typeof(FlipSide);
    }

    public Vector2 Axis
    {
        get => axis;
        set
        {
            axis = value;
            UpdateAxis(Side1Content);
            UpdateAxis(Side2Content);
        }
    }

    public bool IsFlipped
    {
        get { return (bool)GetValue(IsFlippedProperty); }
        set { SetValue(IsFlippedProperty, value); }
    }

    public object Side1
    {
        get { return (object)GetValue(Side1Property); }
        set { SetValue(Side1Property, value); }
    }

    public object Side2
    {
        get { return (object)GetValue(Side2Property); }
        set { SetValue(Side2Property, value); }
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Side1Content = GetTemplateChild("Side1Content") as ContentPresenter;
        Side2Content = GetTemplateChild("Side2Content") as ContentPresenter;
        LayoutRoot = GetTemplateChild("LayoutRoot") as Grid;

        InitComposition();
    }

    private void InitComposition()
    {
        if (Side1Content == null || Side2Content == null || LayoutRoot == null) return;

        s1Visual = ElementCompositionPreview.GetElementVisual(Side1Content);
        s2Visual = ElementCompositionPreview.GetElementVisual(Side2Content);

        var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

        var opacity1Animation = compositor.CreateExpressionAnimation("this.Target.RotationAngleInDegrees > 90 ? 0f : 1f");
        var opacity2Animation = compositor.CreateExpressionAnimation("(this.Target.RotationAngleInDegrees - 180) > 90 ? 1f : 0f");

        s1Visual.StartAnimation("Opacity", opacity1Animation);
        s2Visual.StartAnimation("Opacity", opacity2Animation);

        OnIsFlippedChanged();

        springAnimation1 = compositor.CreateSpringScalarAnimation();
        springAnimation1.DampingRatio = 0.5f;
        springAnimation1.Period = TimeSpan.FromMilliseconds(200);
        springAnimation1.FinalValue = 180f;

        springAnimation2 = compositor.CreateSpringScalarAnimation();
        springAnimation2.DampingRatio = 0.5f;
        springAnimation2.Period = TimeSpan.FromMilliseconds(200);
        springAnimation2.FinalValue = 180f;

        UpdateAxis(Side1Content);
        UpdateAxis(Side2Content);
        UpdateTransformMatrix(LayoutRoot);

        LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
    }

    private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateTransformMatrix(LayoutRoot);
        UpdateAxis(Side1Content);
        UpdateAxis(Side2Content);
    }

    private void OnIsFlippedChanged()
    {
        float f1 = 0f, f2 = 0f;
        if (IsFlipped)
        {
            f1 = 180f;
            f2 = 360f;
            VisualStateManager.GoToState(this, "Slide2", false);
        }
        else
        {
            f1 = 0f;
            f2 = 180f;
            VisualStateManager.GoToState(this, "Slide1", false);
        }
        if (springAnimation1 != null && springAnimation2 != null)
        {
            springAnimation1.FinalValue = f1;
            springAnimation2.FinalValue = f2;
            s1Visual.StartAnimation("RotationAngleInDegrees", springAnimation1);
            s2Visual.StartAnimation("RotationAngleInDegrees", springAnimation2);
        }
        else
        {
            s1Visual.RotationAngleInDegrees = f1;
            s2Visual.RotationAngleInDegrees = f2;
        }
    }

    private void UpdateAxis(FrameworkElement element)
    {
        var visual = ElementCompositionPreview.GetElementVisual(element);
        var size = element.RenderSize.ToVector2();

        visual.CenterPoint = new Vector3(size.X / 2, size.Y / 2, 0f);
        visual.RotationAxis = new Vector3(axis, 0f);
    }

    private void UpdateTransformMatrix(FrameworkElement element)
    {
        var host = ElementCompositionPreview.GetElementVisual(element);
        var size = element.RenderSize.ToVector2();
        if (size.X == 0 || size.Y == 0) return;
        var n = -1f / size.X;

        Matrix4x4 perspective = new Matrix4x4(
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, n,
            0.0f, 0.0f, 0.0f, 1.0f);

        host.TransformMatrix =
            Matrix4x4.CreateTranslation(-size.X / 2, -size.Y / 2, 0f) *
            perspective *
            Matrix4x4.CreateTranslation(size.X / 2, size.Y / 2, 0f);
    }
}
