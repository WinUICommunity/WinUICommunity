namespace WinUICommunity;

public class ShadowTextControl : OutlineTextControl
{
    public static readonly DependencyProperty BlurAmountProperty =
        DependencyProperty.Register(nameof(BlurAmount), typeof(double), typeof(ShadowTextControl), new PropertyMetadata(10d, OnBlurAmountChanged));

    private CompositionMaskBrush _maskBrush;

    public ShadowTextControl()
    {
        _maskBrush = Compositor.CreateMaskBrush();
        var surfaceTextBrush = CreateBrush();
        _maskBrush.Source = surfaceTextBrush;
    }

    public double BlurAmount
    {
        get => (double)GetValue(BlurAmountProperty);
        set => SetValue(BlurAmountProperty, value);
    }

    private Compositor Compositor => ElementCompositionPreview.GetElementVisual(this).Compositor;

    protected virtual CompositionBrush CreateBrush()
    {
        return Compositor.CreateColorBrush(Colors.White);
    }

    protected override void DrawSurfaceCore(CompositionDrawingSurface drawingSurface, float width, float height)
    {
        using (var session = CanvasComposition.CreateDrawingSession(drawingSurface))
        {
            session.Clear(Colors.Transparent);

            StrokeWidth = 0;

            using (var textFormat = new CanvasTextFormat()
            {
                FontSize = (float)FontSize,
                Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                FontWeight = FontWeight,
                FontFamily = FontFamily.Source
            })
            {
                using (var textLayout = new CanvasTextLayout(session, Text, textFormat, width, height))
                {
                    var fullSizeGeometry = CanvasGeometry.CreateRectangle(session, 0, 0, width, height);
                    var textGeometry = CanvasGeometry.CreateText(textLayout);
                    var finalGeometry = fullSizeGeometry.CombineWith(textGeometry, Matrix3x2.Identity, CanvasGeometryCombine.Exclude);
                    using (var layer = session.CreateLayer(1, finalGeometry))
                    {
                        using (var bitmap = new CanvasRenderTarget(session, width, height))
                        {
                            using (var bitmapSession = bitmap.CreateDrawingSession())
                            {
                                DrawText(bitmapSession, textLayout);
                            }
                            using (var blur = new GaussianBlurEffect
                            {
                                BlurAmount = (float)BlurAmount,
                                Source = bitmap,
                                BorderMode = EffectBorderMode.Hard
                            })
                            {
                                session.DrawImage(blur, 0, 0);
                            }
                        }
                    }
                }
            }
        }
    }

    protected virtual void OnBlurAmountChanged(double oldValue, double newValue)
    {
        DrawSurface();
    }

    protected override void UpdateSpriteVisual(SpriteVisual spriteVisual, CompositionDrawingSurface drawingSurface)
    {
        var maskSurfaceBrush = Compositor.CreateSurfaceBrush(DrawingSurface);
        _maskBrush.Mask = maskSurfaceBrush;
        spriteVisual.Brush = _maskBrush;
    }

    private static void OnBlurAmountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var oldValue = (double)args.OldValue;
        var newValue = (double)args.NewValue;
        if (oldValue == newValue)
            return;

        var target = obj as ShadowTextControl;
        target?.OnBlurAmountChanged(oldValue, newValue);
    }
}
