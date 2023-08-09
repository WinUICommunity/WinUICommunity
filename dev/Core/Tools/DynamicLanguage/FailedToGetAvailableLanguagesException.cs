namespace WinUICommunity;
public class FailedToGetAvailableLanguagesException : LocalizerException
{
    public FailedToGetAvailableLanguagesException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
