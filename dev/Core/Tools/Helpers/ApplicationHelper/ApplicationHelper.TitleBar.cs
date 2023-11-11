using Microsoft.UI.Input;

namespace WinUICommunity;
public partial class ApplicationHelper
{
    public static double GetRasterizationScaleForElement(UIElement element)
    {
        if (element.XamlRoot != null)
        {
            return element.XamlRoot.RasterizationScale;
        }
        return 0.0;
    }

    public static void SetDragRegion(Window window, NonClientRegionKind nonClientRegionKind, params FrameworkElement[] frameworkElements)
    {
        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
        List<Windows.Graphics.RectInt32> rects = new List<Windows.Graphics.RectInt32>();

        foreach (var frameworkElement in frameworkElements)
        {
            GeneralTransform transformElement = frameworkElement.TransformToVisual(null);
            Windows.Foundation.Rect bounds = transformElement.TransformBounds(new Windows.Foundation.Rect(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));
            var scale = GetRasterizationScaleForElement(frameworkElement);
            var transparentRect = new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
            rects.Add(transparentRect);
        }

        nonClientInputSrc.SetRegionRects(nonClientRegionKind, rects.ToArray());
    }

    public static void ClearDragRegions(Window window, NonClientRegionKind nonClientRegionKind)
    {
        var noninputsrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
        noninputsrc.ClearRegionRects(nonClientRegionKind);
    }
}
