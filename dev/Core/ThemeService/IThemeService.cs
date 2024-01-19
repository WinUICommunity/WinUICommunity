
namespace WinUICommunity;
public interface IThemeService
{
    delegate void ActualThemeChangedEventHandler(FrameworkElement sender, object args);
    event ActualThemeChangedEventHandler ActualThemeChanged;

    Window Window { get; set; }
    SystemBackdrop CurrentSystemBackdrop { get; set; }
    BackdropType CurrentBackdropType { get; set; }

    void Initialize(Window window, bool useAutoSave = true, string filename = null);
    void ConfigBackdrop(BackdropType backdropType = BackdropType.Mica, bool force = false);
    void ConfigBackdropTintColor();
    void ConfigBackdropTintColor(Color color, bool force = false);
    void ConfigBackdropFallBackColor();
    void ConfigBackdropFallBackColor(Color color, bool force = false);
    void ConfigBackdropTintOpacity();
    void ConfigBackdropTintOpacity(float opacity, bool force = false);
    void ConfigBackdropLuminosityOpacity();
    void ConfigBackdropLuminosityOpacity(float opacity, bool force = false);
    void ConfigElementTheme(ElementTheme elementTheme = ElementTheme.Default, bool force = false);
    void ConfigTitleBar(TitleBarCustomization titleBarCustomization);
    void ConfigBackdropFallBackColorForWindow10(Brush? brush);

    SystemBackdrop GetSystemBackdrop();
    SystemBackdrop GetSystemBackdrop(BackdropType backdropType);
    BackdropType GetBackdropType();
    BackdropType GetBackdropType(SystemBackdrop systemBackdrop);
    ElementTheme GetElementTheme();
    ElementTheme GetActualTheme();
    Color GetDefaultBackdropFallBackColor();
    Color GetBackdropFallBackColor();
    Brush GetBackdropFallBackBrush();
    Color GetDefaultBackdropTintColor();
    Color GetBackdropTintColor();
    Brush GetBackdropTintBrush();
    float GetDefaultBackdropLuminosityOpacity();
    float GetBackdropLuminosityOpacity();
    float GetDefaultBackdropTintOpacity();
    float GetBackdropTintOpacity();

    void SetBackdropType(BackdropType backdropType);
    void SetBackdropLuminosityOpacity(float opacity);
    void SetBackdropTintOpacity(float opacity);
    void SetBackdropFallBackColor(Color color);
    void SetBackdropTintColor(Color color);
    void SetElementTheme(ElementTheme elementTheme);
    void SetElementThemeWithoutSave(ElementTheme elementTheme);

    bool IsDarkTheme();
    void UpdateSystemCaptionButtonForAppWindow(Window window);
    void ResetCaptionButtonColors(Window window);
    void UpdateSystemCaptionButton(Window window);
    void ResetBackdropProperties();
    void OnThemeComboBoxSelectionChanged(object sender);
    void SetThemeComboBoxDefaultItem(ComboBox themeComboBox);
    void OnBackdropComboBoxSelectionChanged(object sender);
    void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox);
    void OnThemeRadioButtonChecked(object sender);
    void SetThemeRadioButtonDefaultItem(Panel ThemePanel);
    void OnBackdropRadioButtonChecked(object sender);
    void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel);
}
