namespace WinUICommunity;

public class LanguageChangedEventArgs : EventArgs
{
    public LanguageChangedEventArgs(string previousLanguage, string currentLanguage)
    {
        PreviousLanguage = previousLanguage;
        CurrentLanguage = currentLanguage;
    }

    public string PreviousLanguage { get; }

    public string CurrentLanguage { get; }
}