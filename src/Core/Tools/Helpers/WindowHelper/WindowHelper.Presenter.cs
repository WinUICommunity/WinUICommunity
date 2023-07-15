namespace WinUICommunity;
public static partial class WindowHelper
{
    public static void SetOverlappedPresenterState(Window window, OverlappedPresenterState overlappedPresenterState)
    {
        if (window.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            switch (overlappedPresenterState)
            {
                case OverlappedPresenterState.Maximized:
                    presenter.Maximize();
                    break;
                case OverlappedPresenterState.Minimized:
                    presenter.Minimize();
                    break;
                case OverlappedPresenterState.Restored:
                    presenter.Restore();
                    break;
            }
        }
    }

    public static void SetOverlappedPresenter(Window window, OverlappedPresenter overlappedPresenter)
    {
        window.AppWindow.SetPresenter(overlappedPresenter);
    }
}
