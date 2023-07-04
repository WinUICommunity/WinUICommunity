using Microsoft.UI.Xaml.Media.Animation;

namespace WinUICommunity;

public partial class NavigationManager
{
    public void GoForward()
    {
        Frame.GoForward();
    }

    public bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
    {
        // Don't open the same page multiple times
        if (pageType != null && Frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(lastParamUsed)))
        {
            var navigationResult = Frame.Navigate(pageType, parameter, infoOverride);
            if (navigationResult)
            {
                lastParamUsed = parameter;
            }

            return navigationResult;
        }
        else
        {
            return false;
        }
    }

    public bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
        where T : Page
    {
        return Navigate(typeof(T), parameter, infoOverride);
    }

    public bool NavigateForJson(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
    {
        if (pageType != null)
        {
            NavigationArgs args = new();
            args.JsonFilePath = this.JsonFilePath;
            args.PathType = this.PathType;
            args.IncludedInBuildMode = this.IncludedInBuildMode;
            args.NavigationView = this.navigationView;
            args.Parameter = parameter;
            return Frame.Navigate(pageType, args, infoOverride);
        }
        else
        {
            return false;
        }
    }

    public bool NavigateForJson<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
        where T : Page
    {
        return NavigateForJson(typeof(T), parameter, infoOverride);
    }

    private void RegisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated += Service_Navigated;
            frame.NavigationFailed += Service_NavigationFailed;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated -= Service_Navigated;
            frame.NavigationFailed -= Service_NavigationFailed;
        }
    }

    private void Service_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        NavigationFailed?.Invoke(sender, e);
    }

    private void Service_Navigated(object sender, NavigationEventArgs e)
    {
        Navigated?.Invoke(sender, e);
    }
}
