using Microsoft.Windows.AppNotifications;
using WinUICommunityGallery.Pages;
using WinUICommunity;

namespace WinUICommunityGallery.AppNotification;
public class ToastWithTextBox : IScenario
{
    private static ToastWithTextBox _Instance;
    public static ToastWithTextBox Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ToastWithTextBox();
            }
            return _Instance;
        }
        set { _Instance = value; }
    }

    public int ScenarioId { get; set; } = 2;
    public string ScenarioName { get; set; } = "Toast with TextBox";

    private const string textBoxReplyId = "textBoxReplyId";
    public void NotificationReceived(AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var notification = NotificationHelper.GetNotificationForWithTextBox(ScenarioName, textBoxReplyId, notificationActivatedEventArgs);
        AppNotificationPage.Instance.NotificationReceived(notification);
    }

    public bool SendToast()
    {
        return ScenarioHelper.SendToastWithTextBox(1, textBoxReplyId, "Send Local Toast with TextBox", "Hi, This is a Local Toast", "Reply", "Pleaser Answer Here...", "Reply", "logo.png");
    }
}
