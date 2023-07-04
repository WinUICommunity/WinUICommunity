using System.Collections.ObjectModel;

namespace WinUICommunity;

public class ControlInfoDataGroup
{
    public ControlInfoDataGroup(string uniqueId, string title, string secondaryTitle, string subtitle, string imagePath,
        string imageIconPath, string description, string apiNamespace, bool isSpecialSection, bool hideGroup,
        bool showItemsWithoutGroup, bool isExpanded, bool isFooterNavigationViewItem, ControlInfoBadge infoBadge)
    {
        UniqueId = uniqueId;
        Title = title;
        SecondaryTitle = secondaryTitle;
        Subtitle = subtitle;
        ImagePath = imagePath;
        ImageIconPath = imageIconPath;
        Description = description;
        ApiNamespace = apiNamespace;
        IsSpecialSection = isSpecialSection;
        HideGroup = hideGroup;
        ShowItemsWithoutGroup = showItemsWithoutGroup;
        IsExpanded = isExpanded;
        IsFooterNavigationViewItem = isFooterNavigationViewItem;
        InfoBadge = infoBadge;
        Items = new ObservableCollection<ControlInfoDataItem>();
    }

    public string UniqueId { get; set; }
    public string Title { get; set; }
    public string SecondaryTitle { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageIconPath { get; set; }
    public string ApiNamespace { get; set; }
    public bool IsSpecialSection { get; set; }
    public bool HideGroup { get; set; }
    public bool ShowItemsWithoutGroup { get; set; }
    public bool IsExpanded { get; set; }
    public bool IsFooterNavigationViewItem { get; set; }
    public ObservableCollection<ControlInfoDataItem> Items { get; set; }
    public ControlInfoBadge InfoBadge { get; set; }
    public override string ToString()
    {
        return Title;
    }
}
