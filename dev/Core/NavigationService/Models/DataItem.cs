using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace WinUICommunity;

public partial class DataItem : BaseDataInfo
{
    public DataItem(string title)
    {
        Title = title;
    }

    public DataItem(string title, string imagePath)
    {
        Title = title;
        ImagePath = imagePath;
    }

    [JsonConstructor]
    public DataItem(string uniqueId, string sectionId, string title, string secondaryTitle, object parameter, string apiNamespace,
        string subtitle, string imagePath, string imageIconPath, string badgeString, string description,
        string content, bool isNew, bool isUpdated, bool isPreview, bool hideItem,
        bool hideNavigationViewItem, bool usexUid, bool isNavigationViewItemHeader, DataInfoBadge dataInfoBadge,
        ObservableCollection<DataLink> links, ObservableCollection<string> extra)
    {
        UniqueId = uniqueId;
        SectionId = sectionId;
        Title = title;
        SecondaryTitle = secondaryTitle;
        Parameter = parameter;
        ApiNamespace = apiNamespace;
        Subtitle = subtitle;
        Description = description;
        ImagePath = imagePath;
        ImageIconPath = imageIconPath;
        BadgeString = badgeString;
        Content = content;
        IsNew = isNew;
        IsUpdated = isUpdated;
        IsPreview = isPreview;
        HideItem = hideItem;
        HideNavigationViewItem = hideNavigationViewItem;
        UsexUid = usexUid;
        IsNavigationViewItemHeader = isNavigationViewItemHeader;
        DataInfoBadge = dataInfoBadge;
        Links = links;
        Extra = extra;
        Items = new ObservableCollection<DataItem>();
    }
    public string UniqueId { get; set; }
    public string SectionId { get; set; }
    public string Title { get; set; }
    public string SecondaryTitle { get; set; }
    public object Parameter { get; set; }
    public string ApiNamespace { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageIconPath { get; set; }
    public string BadgeString { get; set; }
    public string Content { get; set; }
    public bool IsNew { get; set; }
    public bool IsUpdated { get; set; }
    public bool IsPreview { get; set; }
    public bool HideItem { get; set; }
    public bool HideNavigationViewItem { get; set; }
    public ObservableCollection<DataLink> Links { get; set; }
    public ObservableCollection<DataItem> Items { get; set; }
    public ObservableCollection<string> Extra { get; set; }
    public bool IncludedInBuild { get; set; } = true;
    public bool IsNavigationViewItemHeader { get; set; }
    public bool UsexUid { get; set; }
    public override string ToString()
    {
        return Title;
    }
}
