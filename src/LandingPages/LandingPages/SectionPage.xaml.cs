namespace WinUICommunity;
public sealed partial class SectionPage : ItemsPageBase
{
    public SectionPage()
    {
        this.InitializeComponent();
    }

    public void GetData(DataSource dataSource, string uniqueId)
    {
        var group = dataSource.GetGroup(uniqueId);
        if (group != null)
        {
            TitleTxt.Text = group?.Title;
            Items = group.Items?.Where(i => !i.HideItem)?.OrderBy(i => i.Title)?.ToList();
        }
    }

    public async void GetDataAsync(string uniqueId, string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var group = await new DataSource().GetGroupAsync(uniqueId, jsonFilePath, pathType, autoIncludedInBuild);

        if (group != null)
        {
            TitleTxt.Text = group?.Title;
            Items = group.Items?.Where(i => !i.HideItem)?.OrderBy(i => i.Title)?.ToList();
        }
    }

    protected override bool GetIsNarrowLayoutState()
    {
        return LayoutVisualStates.CurrentState == NarrowLayout;
    }
}
