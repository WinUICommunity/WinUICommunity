namespace WinUICommunity;
public class FailedToGetLocalizedStringException : LocalizerException
{
    public FailedToGetLocalizedStringException(string uid, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        Uid = uid;
    }

    public string Uid { get; }
}
