// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using WinRT.Interop;

namespace WinUICommunity;

[TemplatePart(Name = nameof(PART_FooterPresenter), Type = typeof(ContentPresenter))]
[TemplatePart(Name = nameof(PART_ContentPresenter), Type = typeof(ContentPresenter))]
public partial class TitleBar : Control
{
    WindowMessageMonitor? monitor;
    WindowMessageMonitor? inputNonClientPointerSourceMonitor;
    MenuFlyout? MenuFlyout;
    ContentPresenter? PART_ContentPresenter;
    ContentPresenter? PART_FooterPresenter;
    private void SetWASDKTitleBar()
    {
        if (this.Window == null)
        {
            return;
            // TO DO: Throw exception that window has not been set? 
        }
        if (AutoConfigureCustomTitleBar)
        {
            Window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            ConfigPresenter();

            if (this.ContextFlyout is not null && this.ContextFlyout is MenuFlyout menuFlyout)
            {
                this.MenuFlyout = menuFlyout;
                monitor = new WindowMessageMonitor(this.Window);
                monitor.WindowMessageReceived -= Monitor_WindowMessageReceived;
                monitor.WindowMessageReceived += Monitor_WindowMessageReceived;

                var hwnd = WindowNative.GetWindowHandle(this.Window);
                var inputNonClientPointerSourceHandle = NativeMethods.FindWindowEx(hwnd, IntPtr.Zero, "InputNonClientPointerSource", null);

                if (inputNonClientPointerSourceHandle != IntPtr.Zero)
                {
                    var style = NativeMethods.GetWindowLongAuto(hwnd, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE);
                    NativeMethods.SetWindowLongAuto(hwnd, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE, (IntPtr)(style & ~(int)NativeValues.WindowStyle.WS_SYSMENU));

                    inputNonClientPointerSourceMonitor = new WindowMessageMonitor(inputNonClientPointerSourceHandle);
                    inputNonClientPointerSourceMonitor.WindowMessageReceived -= InputNonClientPointerSourceMonitor_WindowMessageReceived;
                    inputNonClientPointerSourceMonitor.WindowMessageReceived += InputNonClientPointerSourceMonitor_WindowMessageReceived;
                }
            }

            this.Window.SizeChanged -= Window_SizeChanged;
            this.Window.SizeChanged += Window_SizeChanged;
            this.Window.Activated -= Window_Activated;
            this.Window.Activated += Window_Activated;

            if (Window.Content is FrameworkElement rootElement)
            {
                UpdateCaptionButtons(rootElement);
                rootElement.ActualThemeChanged += (s, e) =>
                {
                    UpdateCaptionButtons(rootElement);
                };
            }

            PART_ContentPresenter = GetTemplateChild(nameof(PART_ContentPresenter)) as ContentPresenter;
            PART_FooterPresenter = GetTemplateChild(nameof(PART_FooterPresenter)) as ContentPresenter;

            // Get caption button occlusion information.
            int CaptionButtonOcclusionWidthRight = Window.AppWindow.TitleBar.RightInset;
            int CaptionButtonOcclusionWidthLeft = Window.AppWindow.TitleBar.LeftInset;
            PART_LeftPaddingColumn!.Width = new GridLength(CaptionButtonOcclusionWidthLeft);
            PART_RightPaddingColumn!.Width = new GridLength(CaptionButtonOcclusionWidthRight);

            if (DisplayMode == DisplayMode.Tall)
            {
                // Choose a tall title bar to provide more room for interactive elements 
                // like search box or person picture controls.
                Window.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            }
            else
            {
                Window.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
            }
            // Recalculate the drag region for the custom title bar 
            // if you explicitly defined new draggable areas.
            SetDragRegionForCustomTitleBar();

            _isAutoConfigCompleted = true;
        }
    }

    private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        UpdateVisualStateAndDragRegion(args.Size);
    }

    private void UpdateVisualStateAndDragRegion(Windows.Foundation.Size size)
    {
        if (size.Width <= CompactStateBreakpoint)
        {
            if (Content != null || Footer != null)
            {
                VisualStateManager.GoToState(this, NarrowState, true);
            }
        }
        else
        {
            VisualStateManager.GoToState(this, WideState, true);
        }

        SetDragRegionForCustomTitleBar();
    }

    private void UpdateCaptionButtons(FrameworkElement rootElement)
    {
        Window.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Window.AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        if (rootElement.ActualTheme == ElementTheme.Dark)
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
        else
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
    }

    public void ResetWASDKTitleBar()
    {
        if (this.Window == null)
        {
            return;
            // TO DO: Throw exception that window has not been set? 
        }

        // Only reset if we were the ones who configured
        if (_isAutoConfigCompleted)
        {
            Window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = false;
            this.Window.SizeChanged -= Window_SizeChanged;
            this.Window.Activated -= Window_Activated;
            SizeChanged -= this.TitleBar_SizeChanged;
            Window.AppWindow.TitleBar.ResetToDefault();
        }
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            VisualStateManager.GoToState(this, WindowDeactivatedState, true);
        }
        else
        {
            VisualStateManager.GoToState(this, WindowActivatedState, true);
        }
    }

    public void SetDragRegionForCustomTitleBar()
    {
        if (AutoConfigureCustomTitleBar && Window is not null)
        {
            ClearDragRegions(NonClientRegionKind.Passthrough);
            var items = new FrameworkElement?[] { PART_ContentPresenter, PART_FooterPresenter, PART_ButtonHolder };
            var validItems = items.Where(x => x is not null).Select(x => x!).ToArray(); // Prune null items
            SetDragRegion(NonClientRegionKind.Passthrough, validItems);
        }
    }
    
    public void SetDragRegion(NonClientRegionKind nonClientRegionKind, params FrameworkElement[] frameworkElements)
    {
        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(Window.AppWindow.Id);
        List<Windows.Graphics.RectInt32> rects = new List<Windows.Graphics.RectInt32>();
        var scale = GeneralHelper.GetRasterizationScaleForElement(this);

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

    public void ClearDragRegions(NonClientRegionKind nonClientRegionKind)
    {
        var noninputsrc = InputNonClientPointerSource.GetForWindowId(Window.AppWindow.Id);
        noninputsrc.ClearRegionRects(nonClientRegionKind);
    }

    private void Monitor_WindowMessageReceived(object? sender, WindowMessageEventArgs e)
    {
        if (e.MessageType == NativeValues.WindowMessage.WM_SYSMENU)
        {
            e.Result = 0;
            e.Handled = true;
        }
        else if (e.MessageType == NativeValues.WindowMessage.WM_SYSCOMMAND)
        {
            NativeValues.SystemCommand sysCommand = (NativeValues.SystemCommand)(e.Message.WParam & 0xFFF0);

            if (sysCommand is NativeValues.SystemCommand.SC_MOUSEMENU)
            {
                FlyoutShowOptions options = new FlyoutShowOptions();
                options.Position = new Windows.Foundation.Point(0, 15);
                options.ShowMode = FlyoutShowMode.Standard;
                MenuFlyout?.ShowAt(null, options);
                e.Result = 0;
                e.Handled = true;
            }
            else if (sysCommand is NativeValues.SystemCommand.SC_KEYMENU)
            {
                FlyoutShowOptions options = new FlyoutShowOptions();
                options.Position = new Windows.Foundation.Point(0, 45);
                options.ShowMode = FlyoutShowMode.Standard;
                MenuFlyout?.ShowAt(null, options);
                e.Result = 0;
                e.Handled = true;
            }
        }
    }

    private void InputNonClientPointerSourceMonitor_WindowMessageReceived(object? sender, WindowMessageEventArgs e)
    {
        if (e.MessageType == NativeValues.WindowMessage.WM_NCLBUTTONDOWN)
        {
            if (MenuFlyout?.IsOpen ?? false)
            {
                MenuFlyout.Hide();
            }
        }
        else if (e.MessageType == NativeValues.WindowMessage.WM_NCRBUTTONDOWN)
        {
            PointInt32 pt = new PointInt32(Convert.ToInt32(e.Message.LParam) & 0xFFFF, Convert.ToInt32(e.Message.LParam) >> 16);
            FlyoutShowOptions options = new FlyoutShowOptions();
            options.ShowMode = FlyoutShowMode.Standard;
            options.Position = OSVersionHelper.IsWindows11_22000_OrGreater ?
            new Windows.Foundation.Point((pt.X - this.Window.AppWindow.Position.X - 8) / XamlRoot.RasterizationScale, (pt.Y - this.Window.AppWindow.Position.Y) / XamlRoot.RasterizationScale) :
            new Windows.Foundation.Point(pt.X - this.Window.AppWindow.Position.X - 8, pt.Y - this.Window.AppWindow.Position.Y);

            MenuFlyout?.ShowAt(this, options);
            e.Result = 0;
            e.Handled = true;
        }
    }
}
