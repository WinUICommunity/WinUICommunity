
namespace WinUICommunity;
public interface IThemeService
{
    void Initialize(Window window, bool useAutoSave = true, string filename = null);
    void ConfigBackdrop(BackdropType backdropType = BackdropType.Mica, bool force = false);
    void ConfigElementTheme(ElementTheme elementTheme = ElementTheme.Default, bool force = false);
    void ConfigTitleBar(TitleBarCustomization titleBarCustomization);
    void ConfigBackdropFallBackColorForWindow10(Brush? brush);

    delegate void ActualThemeChangedEventHandler(FrameworkElement sender, object args);
    event ActualThemeChangedEventHandler ActualThemeChanged;

    Window CurrentWindow { get; set; }
    SystemBackdrop CurrentSystemBackdrop { get; set; }
    BackdropType CurrentBackdropType { get; set; }
    SystemBackdrop GetSystemBackdrop(BackdropType backdropType);
    SystemBackdrop GetCurrentSystemBackdrop();
    BackdropType GetBackdropType(SystemBackdrop systemBackdrop);
    BackdropType GetCurrentBackdropType();
    ElementTheme GetCurrentTheme();

    void SetCurrentSystemBackdrop(BackdropType backdropType);
    bool IsDarkTheme();
    void UpdateSystemCaptionButtonForAppWindow(Window window);
    void ResetCaptionButtonColors(Window window);
    void UpdateSystemCaptionButton(Window window);
    void SetCurrentTheme(ElementTheme elementTheme);
    void OnThemeComboBoxSelectionChanged(object sender);
    void SetThemeComboBoxDefaultItem(ComboBox themeComboBox);
    void OnBackdropComboBoxSelectionChanged(object sender);
    void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox);
    void OnThemeRadioButtonChecked(object sender);
    void SetThemeRadioButtonDefaultItem(Panel ThemePanel);
    void OnBackdropRadioButtonChecked(object sender);
    void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel);
}
