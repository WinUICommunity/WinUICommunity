namespace WinUICommunity;
public static class WindowExtension
{
    public static void SetOverlappedPresenterState(this Window window, OverlappedPresenterState overlappedPresenterState)
    {
        WindowHelper.SetOverlappedPresenterState(window, overlappedPresenterState);
    }

    public static void SetOverlappedPresenter(this Window window, OverlappedPresenter overlappedPresenter)
    {
        WindowHelper.SetOverlappedPresenter(window, overlappedPresenter);
    }

    public static void SetApplicationLayoutRTL(this Window window)
    {
        ApplicationHelper.SetApplicationLayoutRTL(window);
    }

    public static void SetApplicationLayoutLTR(this Window window)
    {
        ApplicationHelper.SetApplicationLayoutLTR(window);
    }
}
