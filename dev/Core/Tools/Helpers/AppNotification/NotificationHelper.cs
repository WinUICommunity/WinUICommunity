using Microsoft.Windows.AppNotifications;

namespace WindowUI;
public static class NotificationHelper
{
    private const string scenarioTag = "scenarioId";

    public static string ExtractParamFromArgs(string args, string paramName)
    {
        var tag = paramName;
        tag += "=";

        var tagStart = args.IndexOf(tag);
        if (tagStart == -1)
        {
            return null;
        }

        var paramStart = tagStart + tag.Length;

        var paramEnd = args.IndexOf("&", paramStart);
        if (paramEnd == -1)
        {
            paramEnd = args.Length;
        }

        return args.Substring(paramStart, paramEnd - paramStart);
    }

    public static string MakeScenarioIdToken(int id)
    {
        return scenarioTag + "=" + id.ToString();
    }

    public static int ExtractScenarioIdFromArgs(string args)
    {
        var scenarioId = ExtractParamFromArgs(args, scenarioTag);

        return scenarioId == null ? 0 : int.Parse(scenarioId);
    }

    public static Notification GetNotificationForWithAvatar(string scenarioName, AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var notification = new Notification();
        notification.Originator = scenarioName;
        var action = ExtractParamFromArgs(notificationActivatedEventArgs.Argument, "action");
        notification.Action = action == null ? "" : action;
        return notification;
    }
    public static Notification GetNotificationForWithTextBox(string scenarioName, string textBoxReplyId, AppNotificationActivatedEventArgs notificationActivatedEventArgs)
    {
        var input = notificationActivatedEventArgs.UserInput;
        var text = input[textBoxReplyId];

        var notification = new Notification();
        notification.Originator = scenarioName;
        var action = ExtractParamFromArgs(notificationActivatedEventArgs.Argument, "action");
        notification.Action = action == null ? "" : action;
        notification.HasInput = true;
        notification.Input = text;
        return notification;
    }
}
