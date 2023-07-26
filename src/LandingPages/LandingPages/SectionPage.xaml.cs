namespace WinUICommunity;
public sealed partial class SectionPage : ItemsPageBase
{
    public SectionPage()
    {
        this.InitializeComponent();
    }

    public void OrderBy(Func<DataItem, object> orderby = null)
    {
        if (orderby != null)
        {
            Items = Items?.OrderBy(orderby);
        }
        else
        {
            Items = Items?.OrderBy(i => i.Title);
        }
    }

    public void OrderByDescending(Func<DataItem, object> orderByDescending = null)
    {
        if (orderByDescending != null)
        {
            Items = Items?.OrderByDescending(orderByDescending);
        }
        else
        {
            Items = Items?.OrderByDescending(i => i.Title);
        }
    }

    public void GetData(DataSource dataSource, string uniqueId)
    {
        var group = dataSource.GetSectionGroup(uniqueId);
        GetItems(group);
    }

    public async void GetDataAsync(string uniqueId, string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var group = await new DataSource().GetSectionGroupAsync(uniqueId, jsonFilePath, pathType, autoIncludedInBuild);

        GetItems(group);
    }

    private void GetItems(DataGroup group)
    {
        if (group != null)
        {
            TitleTxt.Text = group?.Title;

            var items = group.Items?.Where(i => !i.HideItem);

            Items = items?.ToList();
        }
    }

    protected override bool GetIsNarrowLayoutState()
    {
        return LayoutVisualStates.CurrentState == NarrowLayout;
    }
}
