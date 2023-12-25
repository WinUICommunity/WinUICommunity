namespace WinUICommunity;

[MarkupExtensionReturnType(ReturnType = typeof(FontIconSource))]
public class FontIconSourceExtension : TextIconExtension
{
    /// <summary>
    /// Gets or sets the <see cref="string"/> value representing the icon to display.
    /// </summary>
    public string? Glyph { get; set; }

    /// <summary>
    /// Gets or sets the font family to use to display the icon. If <see langword="null"/>, "Segoe MDL2 Assets" will be used.
    /// </summary>
    public FontFamily? FontFamily { get; set; }

    /// <inheritdoc/>
    protected override object ProvideValue()
    {
        var fontIcon = new FontIconSource
        {
            Glyph = Glyph,
            FontFamily = FontFamily ?? SymbolThemeFontFamily,
            FontWeight = FontWeight,
            FontStyle = FontStyle,
            IsTextScaleFactorEnabled = IsTextScaleFactorEnabled,
            MirroredWhenRightToLeft = MirroredWhenRightToLeft
        };

        if (FontSize > 0)
        {
            fontIcon.FontSize = FontSize;
        }

        if (Foreground != null)
        {
            fontIcon.Foreground = Foreground;
        }

        return fontIcon;
    }
}
