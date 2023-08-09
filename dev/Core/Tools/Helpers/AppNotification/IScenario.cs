using Microsoft.Windows.AppNotifications;

namespace WinUICommunity;
public interface IScenario
{
    public int ScenarioId { get; set; }
    public string ScenarioName { get; set; }
    public bool SendToast();
    public void NotificationReceived(AppNotificationActivatedEventArgs notificationActivatedEventArgs);
}
