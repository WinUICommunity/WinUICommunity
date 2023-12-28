namespace WinUICommunity;
public class LocalizerIsAlreadyBuiltException : LocalizerException
{
    public LocalizerIsAlreadyBuiltException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
