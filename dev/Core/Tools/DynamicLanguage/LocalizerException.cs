namespace WindowUI;

public class LocalizerException : Exception
{
    public LocalizerException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
