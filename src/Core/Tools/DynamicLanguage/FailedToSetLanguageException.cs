namespace WinUICommunity;
public class FailedToSetLanguageException : LocalizerException
{
    public FailedToSetLanguageException(string oldLanguage, string newLanguage, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        OldLanguage = oldLanguage;
        NewLanguage = newLanguage;
    }

    public string OldLanguage { get; }

    public string NewLanguage { get; }
}
