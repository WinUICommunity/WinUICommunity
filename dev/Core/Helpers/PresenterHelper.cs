namespace WinUICommunity;
public partial class PresenterHelper
{
    public static void SetOverlappedPresenterState(AppWindow appWindow, OverlappedPresenterState overlappedPresenterState)
    {
        if (appWindow.Presenter is OverlappedPresenter presenter)
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

    public static void SetOverlappedPresenter(AppWindow appWindow, OverlappedPresenter overlappedPresenter)
    {
        appWindow.SetPresenter(overlappedPresenter);
    }
}
