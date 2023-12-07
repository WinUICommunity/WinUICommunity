namespace WinUICommunity;
public interface IRainbowFrameService
{
    void Initialize(Window window, TimeSpan frameUpdateInterval, int effectSpeed);
    void Initialize(Window window, TimeSpan frameUpdateInterval);
    void Initialize(Window window, int effectSpeed);
    void Initialize(Window window);
    void UpdateEffectSpeed(int effectSpeed);
    void ResetFrameColorToDefault();
    void ChangeFrameColor(Color color);
    void ChangeFrameColor(uint color);
    void StartRainbowFrame();
    void StopRainbowFrame();
}
