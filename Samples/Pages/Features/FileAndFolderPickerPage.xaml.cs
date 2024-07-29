using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;
public sealed partial class FileAndFolderPickerPage : Page
{
    public FileAndFolderPickerPage()
    {
        this.InitializeComponent();
    }

    private async void btnPickSaveFileAsync_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var ext = new Dictionary<string, IList<string>>();
        ext.Add("Plain Text", new List<string>() { ".txt" });
        var picker = await FileAndFolderPickerHelper.PickSaveFileAsync(App.CurrentWindow, ext);
        if (picker != null)
        {
            txtRes1.Text = picker.Path;
        }
    }

    private async void btnPickMultipleFilesAsync_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var fileTypeFilter = new List<string> { ".txt", ".rtf" };
        var picker = await FileAndFolderPickerHelper.PickMultipleFilesAsync(App.CurrentWindow, fileTypeFilter);
        if (picker != null)
        {
            foreach (var item in picker)
            {
                txtRes2.Text = item.Path;
            }
        }
    }

    private async void btnPickSingleFileAsync_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var fileTypeFilter = new List<string> { ".txt", ".rtf" };
        var picker = await FileAndFolderPickerHelper.PickSingleFileAsync(App.CurrentWindow, fileTypeFilter);
        if (picker != null)
        {
            txtRes3.Text = picker.Path;
        }
    }

    private async void btnPickSingleFolderAsync_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var picker = await FileAndFolderPickerHelper.PickSingleFolderAsync(App.CurrentWindow);
        if (picker != null)
        {
            txtRes4.Text = picker.Path;
        }
    }
}
