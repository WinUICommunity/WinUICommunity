using WinRT.Interop;

namespace WinUICommunity;
public class RainbowFrameHelper
{
    private uint _defaultColor = 0xFFFFFFFF;
    private DispatcherTimer _frameTimer;
    private DateTimeOffset _started;
    private TimeSpan FrameUpdateInterval = TimeSpan.FromMilliseconds(16);
    private Window _window;
    private IntPtr hwnd;
    private int EffectSpeed = 4;
    public RainbowFrameHelper(Window window, TimeSpan frameUpdateInterval, int effectSpeed)
    {
        InitializeWindow(window);
        InitializeEffectSpeed(effectSpeed);
        FrameUpdateInterval = frameUpdateInterval;
    }
    public RainbowFrameHelper(Window window, TimeSpan frameUpdateInterval)
    {
        InitializeWindow(window);
        FrameUpdateInterval = frameUpdateInterval;
    }

    public RainbowFrameHelper(Window window, int effectSpeed)
    {
        InitializeWindow(window);
        InitializeEffectSpeed(effectSpeed);
    }

    public RainbowFrameHelper(Window window)
    {
        InitializeWindow(window);
    }

    private void InitializeWindow(Window window)
    {
        _window = window;
        hwnd = WindowNative.GetWindowHandle(_window);
    }

    private void InitializeEffectSpeed(int effectSpeed)
    {
        if (effectSpeed > 0)
        {
            EffectSpeed = effectSpeed;
        }
    }

    /// <summary>
    /// default value is 16ms
    /// </summary>
    /// <param name="frameUpdateInterval"></param>
    public void UpdateFrameUpdateInterval(TimeSpan frameUpdateInterval)
    {
        FrameUpdateInterval = frameUpdateInterval;
    }

    /// <summary>
    /// default value is 4 and effectSpeed should be greater than zero (0)
    /// </summary>
    /// <param name="effectSpeed"></param>
    public void UpdateEffectSpeed(int effectSpeed)
    {
        InitializeEffectSpeed(effectSpeed);
    }

    public void ResetFrameColorToDefault()
    {
        _frameTimer?.Stop();
        _frameTimer = null;
        ChangeFrameColor(_defaultColor);
    }

    public void ChangeFrameColor(Color color)
    {
        _frameTimer?.Stop();
        _frameTimer = null;
        ChangeFrameColor(GeneralHelper.ColorToUInt(color));
    }

    public void ChangeFrameColor(uint color)
    {
        try
        {
            if (OSVersionHelper.IsWindows11_22000_OrGreater)
            {
                NativeMethods.DwmSetWindowAttribute(hwnd, NativeValues.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref color, sizeof(uint));
            }
        }
        catch (Exception)
        {
        }
    }

    public void StartRainbowFrame()
    {
        _started = DateTimeOffset.Now;

        if (_frameTimer == null)
        {
            _frameTimer = new DispatcherTimer();
            _frameTimer.Interval = FrameUpdateInterval;
            _frameTimer.Tick += (s, e) => UpdateFrameColor(s, e);
        }
        _frameTimer.Start();
    }

    public void StopRainbowFrame()
    {
        _frameTimer?.Stop();
    }

    private void UpdateFrameColor(object sender, object e)
    {
        var saturateAndToColor = new Func<float, float, float, uint>((a, b, c) =>
        {
            return GeneralHelper.ColorToUInt(new Color
            {
                R = (byte)(255f * Math.Clamp(a, 0f, 1f)),
                G = (byte)(255f * Math.Clamp(b, 0f, 1f)),
                B = (byte)(255f * Math.Clamp(c, 0f, 1f))
            });
        });

        // Helper for converting a hue [0, 1) to an RGB value.
        // Credit to https://www.chilliant.com/rgb2hsv.html
        var hueToRGB = new Func<float, uint>(H =>
        {
            float R = Math.Abs(H * 6 - 3) - 1;
            float G = 2 - Math.Abs(H * 6 - 2);
            float B = 2 - Math.Abs(H * 6 - 4);
            return saturateAndToColor(R, G, B);
        });

        // Now, the main body of work.
        // - Convert the time delta between when we were started and now, to a hue. This will cycle us through all the colors.
        // - Convert that hue to an RGB value.
        // - Set the frame's color to that RGB color.
        var now = DateTimeOffset.Now;
        var delta = now - _started;
        var seconds = delta.TotalSeconds / EffectSpeed; // divide by EffectSpeed (Default = 4), to make the effect slower. Otherwise it flashes way too fast.

        double integerValue = Math.Floor(seconds);

        double decimalValue = seconds - integerValue;

        var color = hueToRGB((float)decimalValue);

        ChangeFrameColor(color);
    }
}
