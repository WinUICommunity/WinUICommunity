namespace WinUICommunity;

public sealed class WindowMessageEventArgs : EventArgs
{
    internal WindowMessageEventArgs(nint hwnd, uint messageId, nuint wParam, nint lParam)
    {
        Message = new Message(hwnd, messageId, wParam, lParam);
    }

    public nint Result { get; set; }

    public bool Handled { get; set; }

    public Message Message { get; }

    internal NativeValues.WindowMessage MessageType => (NativeValues.WindowMessage)Message.MessageId;
}
