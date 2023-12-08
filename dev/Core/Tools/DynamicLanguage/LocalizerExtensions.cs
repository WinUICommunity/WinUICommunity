namespace WindowUI;

public static class LocalizerExtensions
{
    public static string GetLocalizedString(this string uid)
    {
        return Localizer.Get().GetLocalizedString(uid);
    }
}