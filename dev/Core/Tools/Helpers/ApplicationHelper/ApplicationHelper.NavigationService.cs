namespace WinUICommunity;
public static partial class ApplicationHelper
{
    public static Type GetPageType(string uniqueId, string assemblyString)
    {
        Assembly assembly;

        if (string.IsNullOrEmpty(assemblyString))
        {
            assembly = Application.Current.GetType().Assembly;
        }
        else
        {
            try
            {
                assembly = Assembly.Load(assemblyString);
            }
            catch (Exception)
            {
                assembly = Application.Current.GetType().Assembly;
            }
        }

        if (assembly is not null)
        {
            return assembly.GetType(uniqueId);
        }
        return null;
    }

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
