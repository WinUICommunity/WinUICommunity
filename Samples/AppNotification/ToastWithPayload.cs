using Microsoft.Windows.AppNotifications;

using WinUICommunityGallery.Pages;
using WinUICommunity;

namespace WinUICommunityGallery.AppNotification;
public class ToastWithPayload : IScenario
{
    private static ToastWithPayload _Instance;

    public static ToastWithPayload Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ToastWithPayload();
            }
            return _Instance;
        }
        set => _Instance = value;
    }

    public int ScenarioId { get; set; } = 3;
    public string ScenarioName { get; set; } = "Toast with Payload";

    public void NotificationReceived(AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var input = notificationActivatedEventArgs.UserInput;
        var text = input["textBoxReplyId"];

        var notification = new Notification();
        notification.Originator = ScenarioName;
        var action = NotificationHelper.ExtractParamFromArgs(notificationActivatedEventArgs.Argument, "action");
        notification.Action = action == null ? "" : action;
        notification.HasInput = true;
        notification.Input = text;
        //MainPage.Current.NotificationReceived(notification);
        WindowHelper.SwitchToThisWindow(App.currentWindow);
        AppNotificationPage.Instance.NotificationReceived(notification);
    }

    public bool SendToast()
    {
        var ScenarioIdToken = NotificationHelper.MakeScenarioIdToken(ScenarioId);

        var xmlPayload = new string(
            "<toast launch = \"action=ToastClick&amp;" + ScenarioIdToken + "\">"
        + "<visual>"
        + "<binding template = \"ToastGeneric\">"
        + "<image placement = \"appLogoOverride\" hint-crop=\"circle\" src = \"" + PathHelper.GetFullPathToAsset("GalleryHeaderImage.png") + "\"/>"
        + "<text>" + ScenarioName + "</text>"
        + "<text>This is an example message using XML</text>"
        + "</binding>"
        + "</visual>"
        + "<actions>"
        + "<input "
        + "id = \"" + "textBoxReplyId" + "\" "
        + "type = \"text\" "
        + "placeHolderContent = \"Type a reply\"/>"
        + "<action "
        + "content = \"Reply\" "
        + "arguments = \"action=Reply&amp;" + ScenarioIdToken + "\" "
        + "hint-inputId = \"" + "textBoxReplyId" + "\"/>"
        + "</actions>"
        + "</toast>");

        return ScenarioHelper.SendToast(xmlPayload);
    }
}
