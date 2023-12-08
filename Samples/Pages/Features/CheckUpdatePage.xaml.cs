using Microsoft.UI.Xaml.Controls;
using System;

namespace WindowUI.DemoApp.Pages;

public sealed partial class CheckUpdatePage : Page
{
    public CheckUpdatePage()
    {
        this.InitializeComponent();
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtRepo.Text))
        {
            txtUser.Text = "WindowUIOrg";
            txtRepo.Text = "WindowUI";
        }
        var ver = await UpdateHelper.CheckUpdateAsync(txtUser.Text, txtRepo.Text);
        if (ver.IsExistNewVersion)
        {
            txtReleaseUrl.Text = $"Release Url: {ver.HtmlUrl}";
            txtCreatedAt.Text = $"Created At: {ver.CreatedAt}";
            txtPublishedAt.Text = $"Published At {ver.PublishedAt}";
            txtIsPreRelease.Text = $"Is PreRelease: {ver.IsPreRelease}";
            txtTagName.Text = $"Tag Name: {ver.TagName}";
            txtChangelog.Text = $"Changelog: {ver.Changelog}";
            foreach (var item in ver.Assets)
            {
                listView.Items.Add($"{item.Url}{Environment.NewLine}Size: {item.Size}");
            }
        }
    }
}
