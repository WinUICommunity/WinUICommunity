using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace WinUICommunity;

public class ControlInfoDataItem
{
    public ControlInfoDataItem(string title, string imagePath)
    {
        Title = title;
        ImagePath = imagePath;
    }

    [JsonConstructor]
    public ControlInfoDataItem(string uniqueId, string title, string secondaryTitle, string apiNamespace, string subtitle,
        string imagePath, string imageIconPath, string badgeString, string description, string content,
        bool isNew, bool isUpdated, bool isPreview, bool hideItem, bool hideNavigationViewItem, bool hideSourceCodeAndRelatedControls,
        ControlInfoBadge infoBadge, ObservableCollection<ControlInfoDocLink> docs, ObservableCollection<string> relatedControls)
    {
        UniqueId = uniqueId;
        Title = title;
        SecondaryTitle = secondaryTitle;
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
        HideSourceCodeAndRelatedControls = hideSourceCodeAndRelatedControls;
        InfoBadge = infoBadge;
        Docs = docs;
        RelatedControls = relatedControls;
        Items = new ObservableCollection<ControlInfoDataItem>();
    }
    public string UniqueId { get; set; }
    public string Title { get; set; }
    public string SecondaryTitle { get; set; }
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
    public bool HideSourceCodeAndRelatedControls { get; set; }
    public ObservableCollection<ControlInfoDocLink> Docs { get; set; }
    public ObservableCollection<ControlInfoDataItem> Items { get; set; }
    public ObservableCollection<string> RelatedControls { get; set; }
    public ControlInfoBadge InfoBadge { get; set; }
    public bool IncludedInBuild { get; set; }
    public override string ToString()
    {
        return Title;
    }
}
