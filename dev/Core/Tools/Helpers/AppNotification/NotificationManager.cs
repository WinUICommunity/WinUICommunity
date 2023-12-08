using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

namespace WindowUI;
public class NotificationManager
{
    private Action<string> _onNotificationInvoked;
    private bool m_isRegistered;

    private Dictionary<int, Action<AppNotificationActivatedEventArgs>> c_notificationHandlers;

    public NotificationManager(Dictionary<int, Action<AppNotificationActivatedEventArgs>> notificationHandlers)
    {
        c_notificationHandlers = notificationHandlers;
    }

    ~NotificationManager()
    {
        Unregister();
    }
    public void Init(NotificationManager notificationManager, Action<string> onNotificationInvoked)
    {
        _onNotificationInvoked = onNotificationInvoked;
        var _notificationManager = AppNotificationManager.Default;

        // To ensure all Notification handling happens in this process instance, register for
        // NotificationInvoked before calling Register(). Without this a new process will
        // be launched to handle the notification.
        _notificationManager.NotificationInvoked += OnNotificationInvoked;

        _notificationManager.Register();
        m_isRegistered = true;

        var currentInstance = AppInstance.GetCurrent();
        if (currentInstance.IsCurrent)
        {
            // AppInstance.GetActivatedEventArgs will report the correct ActivationKind,
            // even in WinUI's OnLaunched.
            AppActivationArguments activationArgs = currentInstance.GetActivatedEventArgs();
            if (activationArgs != null)
            {
                ExtendedActivationKind extendedKind = activationArgs.Kind;
                if (extendedKind == ExtendedActivationKind.AppNotification)
                {
                    var notificationActivatedEventArgs = (AppNotificationActivatedEventArgs)activationArgs.Data;
                    notificationManager.ProcessLaunchActivationArgs(notificationActivatedEventArgs);
                }
            }
        }
    }

    public void Unregister()
    {
        if (m_isRegistered)
        {
            AppNotificationManager.Default.Unregister();
            m_isRegistered = false;
        }
    }

    public void ProcessLaunchActivationArgs(AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        DispatchNotification(notificationActivatedEventArgs);
    }

    public bool DispatchNotification(AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var scenarioId = NotificationHelper.ExtractScenarioIdFromArgs(notificationActivatedEventArgs.Argument);
        if (scenarioId != 0)
        {
            try
            {
                c_notificationHandlers[scenarioId](notificationActivatedEventArgs);
                return true;
            }
            catch
            {
                return false; // Couldn't find a NotificationHandler for scenarioId.
            }
        }
        else
        {
            return false; // No scenario specified in the notification
        }
    }

    void OnNotificationInvoked(object sender, AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        _onNotificationInvoked.Invoke("Notification Received");
        if (!DispatchNotification(notificationActivatedEventArgs))
        {
            _onNotificationInvoked.Invoke("Unrecognized Toast Originator");
        }
    }
}
