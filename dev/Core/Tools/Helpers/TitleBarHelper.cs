using Microsoft.UI.Input;

namespace WindowUI;
public partial class TitleBarHelper
{
    public static void SetDragRegion(Window window, NonClientRegionKind nonClientRegionKind, params FrameworkElement[] frameworkElements)
    {
        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
        List<Windows.Graphics.RectInt32> rects = new List<Windows.Graphics.RectInt32>();
        var scale = GeneralHelper.GetRasterizationScaleForElement(frameworkElements[0]);

        foreach (var frameworkElement in frameworkElements)
        {
            if (frameworkElement == null)
            {
                continue;
            }

            GeneralTransform transformElement = frameworkElement.TransformToVisual(null);
            Windows.Foundation.Rect bounds = transformElement.TransformBounds(new Windows.Foundation.Rect(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));
            var transparentRect = new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
            rects.Add(transparentRect);
        }

        if (rects.Count > 0)
        {
            nonClientInputSrc.SetRegionRects(nonClientRegionKind, rects.ToArray());
        }
    }

    public static void ClearDragRegions(Window window, NonClientRegionKind nonClientRegionKind)
    {
        var noninputsrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
        noninputsrc.ClearRegionRects(nonClientRegionKind);
    }
}
