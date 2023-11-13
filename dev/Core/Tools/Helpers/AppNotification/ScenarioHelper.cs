using Microsoft.Windows.AppNotifications;

namespace WinUICommunity;
public static class ScenarioHelper
{
    public static bool SendToast(string xmlPayload)
    {
        var toast = new AppNotification(xmlPayload);
        AppNotificationManager.Default.Show(toast);

        return toast.Id != 0;
    }

    /// <summary>
    /// Send Local Toast With Avatar
    /// </summary>
    /// <param name="scenarioId">1</param>
    /// <param name="scenarioName">Send Local Toast With Avatar</param>
    /// <param name="message">Hi, This is a Local Toast with Avatar</param>
    /// <param name="buttonContent">Open my App</param>
    /// <param name="actionName">OpenApp</param>
    /// <param name="logoNameWithExtension">Icon.png</param>
    /// <returns></returns>
    public static bool SendToastWithAvatar(int scenarioId, string scenarioName, string message, string buttonContent, string actionName, string logoNameWithExtension)
    {
        var ScenarioIdToken = NotificationHelper.MakeScenarioIdToken(scenarioId);

        var xmlPayload = new string(
            "<toast launch = \"action=ToastClick&amp;" + ScenarioIdToken + "\">"
        + "<visual>"
        + "<binding template = \"ToastGeneric\">"
        + "<image placement = \"appLogoOverride\" hint-crop=\"circle\" src = \"" + PathHelper.GetFullPathToAsset(logoNameWithExtension) + "\"/>"
        + "<text>" + scenarioName + "</text>"
        + "<text>" + message + "</text>"
        + "</binding>"
        + "</visual>"
        + "<actions>"
        + "<action "
        + "content = \"" + buttonContent + "\" "
        + "arguments = \"action=" + actionName + "&amp;" + ScenarioIdToken + "\"/>"
        + "</actions>"
        + "</toast>");

        return SendToast(xmlPayload);
    }

    /// <summary>
    /// Send Local Toast with TextBox
    /// </summary>
    /// <param name="scenarioId">1</param>
    /// <param name="textBoxReplyId">textboxReply</param>
    /// <param name="scenarioName">Send Local Toast with TextBox</param>
    /// <param name="message">Hi, This is a Local Toast with TextBox</param>
    /// <param name="replayButtonContent">Replay</param>
    /// <param name="textBoxPlaceHolder">Please write your answer here!</param>
    /// <param name="actionName">Replay</param>
    /// <param name="logoNameWithExtension">Icon.png</param>
    /// <returns></returns>
    public static bool SendToastWithTextBox(int scenarioId, string textBoxReplyId, string scenarioName, string message, string replayButtonContent, string textBoxPlaceHolder, string actionName, string logoNameWithExtension)
    {
        var ScenarioIdToken = NotificationHelper.MakeScenarioIdToken(scenarioId);

        var xmlPayload = new string(
            "<toast launch = \"action=ToastClick&amp;" + ScenarioIdToken + "\">"
        + "<visual>"
        + "<binding template = \"ToastGeneric\">"
        + "<image placement = \"appLogoOverride\" hint-crop=\"circle\" src = \"" + PathHelper.GetFullPathToAsset(logoNameWithExtension) + "\"/>"
        + "<text>" + scenarioName + "</text>"
        + "<text>" + message + "</text>"
        + "</binding>"
        + "</visual>"
        + "<actions>"
        + "<input "
        + "id = \"" + textBoxReplyId + "\" "
        + "type = \"text\" "
        + "placeHolderContent = \"" + textBoxPlaceHolder + "\"/>"
        + "<action "
        + "content = \"" + replayButtonContent + "\" "
        + "arguments = \"action=" + actionName + "&amp;" + ScenarioIdToken + "\" "
        + "hint-inputId = \"" + textBoxReplyId + "\"/>"
        + "</actions>"
        + "</toast>");

        return SendToast(xmlPayload);
    }
}
