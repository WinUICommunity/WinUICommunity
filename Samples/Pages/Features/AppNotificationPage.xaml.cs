using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUICommunityGallery.AppNotification;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;
public sealed partial class AppNotificationPage : Page
{
    internal static AppNotificationPage Instance;
    public AppNotificationPage()
    {
        this.InitializeComponent();
        Instance = this;
    }

    public void NotificationReceived(Notification notification)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            if (notification.HasInput)
            {
                txtReceived.Text = notification.Input;
            }
            else
            {
                txtReceived.Text = "Notification Received";
            }
        });
    }
    public void NotificationInvoked(string message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            txtInvoked.Text = message;
        });
    }
    private void btnToast1_Click(object sender, RoutedEventArgs e)
    {
        if (!PackageHelper.IsPackaged)
        {
            ToastWithAvatar.Instance.SendToast();
        }
    }

    private void btnToast2_Click(object sender, RoutedEventArgs e)
    {
        if (!PackageHelper.IsPackaged)
        {
            ToastWithTextBox.Instance.SendToast();
        }
    }

    private void btnToast3_Click(object sender, RoutedEventArgs e)
    {
        if (!PackageHelper.IsPackaged)
        {
            ToastWithPayload.Instance.SendToast();
        }
    }
}
