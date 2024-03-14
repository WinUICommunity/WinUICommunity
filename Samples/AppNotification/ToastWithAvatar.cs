using Microsoft.Windows.AppNotifications;
using WinUICommunityGallery.Pages;
using WinUICommunity;

namespace WinUICommunityGallery.AppNotification;
public class ToastWithAvatar : IScenario
{
    private static ToastWithAvatar _Instance;

    public static ToastWithAvatar Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ToastWithAvatar();
            }
            return _Instance;
        }
        set { _Instance = value; }
    }

    public int ScenarioId { get; set; } = 1;
    public string ScenarioName { get; set; } = "Toast with Avatar";

    public void NotificationReceived(AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var notification = NotificationHelper.GetNotificationForWithAvatar(ScenarioName, notificationActivatedEventArgs);
        AppNotificationPage.Instance.NotificationReceived(notification);
    }

    public bool SendToast()
    {
        return ScenarioHelper.SendToastWithAvatar(ScenarioId, ScenarioName, "Hi, This is a Toast", "Open App", "OpenApp", "logo.png");
    }
}
