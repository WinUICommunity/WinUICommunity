using System.Collections.ObjectModel;

namespace WinUICommunity;

public partial class DataGroup : BaseDataInfo
{
    public DataGroup(string uniqueId, string sectionId, string title, string secondaryTitle,
        string subtitle, string imagePath, string imageIconPath, string description,
        string apiNamespace, bool isSpecialSection, bool hideGroup, bool showItemsWithoutGroup,
        bool isExpanded, bool isFooterNavigationViewItem, bool isNavigationViewItemHeader, bool usexUid, DataInfoBadge dataInfoBadge)
    {
        UniqueId = uniqueId;
        SectionId = sectionId;
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
        IsNavigationViewItemHeader = isNavigationViewItemHeader;
        UsexUid = usexUid;
        Order = false;
        OrderByDescending = false;
        DataInfoBadge = dataInfoBadge;
        Items = new ObservableCollection<DataItem>();
    }

    public string UniqueId { get; set; }
    public string SectionId { get; set; }
    public string ApiNamespace { get; set; }
    public string Title { get; set; }
    public string SecondaryTitle { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageIconPath { get; set; }
    public bool IsSpecialSection { get; set; }
    public bool IsExpanded { get; set; }
    public bool HideGroup { get; set; }
    public bool ShowItemsWithoutGroup { get; set; }
    public bool IsFooterNavigationViewItem { get; set; }
    public bool IsNavigationViewItemHeader { get; set; }
    public bool UsexUid { get; set; }
    public bool Order { get; set; }
    public bool OrderByDescending { get; set; }
    public ObservableCollection<DataItem> Items { get; set; }
    public override string ToString()
    {
        return Title;
    }
}
