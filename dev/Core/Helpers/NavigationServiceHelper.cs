namespace WinUICommunity;
public partial class NavigationServiceHelper
{
    public static (string UniqueId, string SectionId) GetUniqueIdAndSectionId(object parameter)
    {
        var uniqueId = string.Empty;
        var sectionId = string.Empty;

        var dataGroup = parameter as DataGroup;
        var dataItem = parameter as DataItem;

        if (dataGroup != null)
        {
            uniqueId = dataGroup.UniqueId;
            sectionId = dataGroup.SectionId;
        }

        if (dataItem != null)
        {
            uniqueId = dataItem.UniqueId;
            sectionId = dataItem.SectionId;
        }

        return (uniqueId, sectionId);
    }
}
