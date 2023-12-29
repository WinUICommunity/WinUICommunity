namespace WinUICommunity;

internal interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
