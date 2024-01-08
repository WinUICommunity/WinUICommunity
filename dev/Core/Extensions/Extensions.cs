namespace WinUICommunity;
public static class Extensions
{
    public static SolidColorBrush GetSolidColorBrush(this string hex)
    {
        return GeneralHelper.GetSolidColorBrush(hex);
    }
}
