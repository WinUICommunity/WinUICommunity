using System.Collections.ObjectModel;
using System.Text.Json;

namespace WinUICommunity;

public class DataSource
{
    private static readonly object _lock = new();

    public ObservableCollection<DataGroup> Groups { get; set; } = new ObservableCollection<DataGroup>();
    
    public async Task<IEnumerable<DataGroup>> GetGroupsAsync(string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetDataAsync(jsonFilePath, pathType, autoIncludedInBuild);

        return Groups;
    }

    public DataGroup GetGroup(string uniqueId)
    {
        var matches = Groups.Where((group) => !string.IsNullOrEmpty(group.UniqueId) && group.UniqueId.Equals(uniqueId));
        if (matches.Count() == 1) return matches.First();
        return null;
    }

    public async Task<DataGroup> GetGroupAsync(string uniqueId, string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetDataAsync(jsonFilePath, pathType, autoIncludedInBuild);
        // Simple linear search is acceptable for small data sets
        var matches = Groups.Where((group) => !string.IsNullOrEmpty(group.UniqueId) && group.UniqueId.Equals(uniqueId));
        return matches.Count() == 1 ? matches.First() : null;
    }

    public DataItem GetItem(string uniqueId)
    {
        // Simple linear search is acceptable for small data sets
        var matches = Groups.SelectMany(group => group.Items).Where((item) => !string.IsNullOrEmpty(item.UniqueId) && item.UniqueId.Equals(uniqueId));
        if (matches.Count() > 0) return matches.First();
        return null;
    }

    public async Task<DataItem> GetItemAsync(string uniqueId, string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetDataAsync(jsonFilePath, pathType, autoIncludedInBuild);
        // Simple linear search is acceptable for small data sets
        var matches = FindItems(Groups.SelectMany(group => group.Items), uniqueId).ToList();

        return matches.Count() > 0 ? matches.First() : null;
    }

    public DataGroup GetGroupFromItem(string uniqueId)
    {
        var matches = Groups.Where((group) => group.Items.FirstOrDefault(item => !string.IsNullOrEmpty(item.UniqueId) && item.UniqueId.Equals(uniqueId)) != null);
        if (matches.Count() == 1) return matches.First();
        return null;
    }

    private IEnumerable<DataItem> FindItems(IEnumerable<DataItem> items, string uniqueId)
    {
        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(item.UniqueId) && item.UniqueId.Equals(uniqueId))
            {
                yield return item;
            }

            if (item.Items != null && item.Items.Any())
            {
                var nestedMatches = FindItems(item.Items, uniqueId);
                foreach (var nestedMatch in nestedMatches)
                {
                    yield return nestedMatch;
                }
            }
        }
    }

    public async Task GetDataAsync(string jsonFilePath, PathType pathType, bool autoIncludedInBuild)
    {
        lock (_lock)
        {
            if (this.Groups.Count() != 0)
            {
                return;
            }
        }

        string filePath = await FileLoaderHelper.GetPath(jsonFilePath, pathType);
        using FileStream openStream = File.OpenRead(filePath);
        var controlInfoDataGroup = await JsonSerializer.DeserializeAsync<DataSource>(openStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lock (_lock)
        {
            UpdateItemInGroups(controlInfoDataGroup.Groups, autoIncludedInBuild);

            foreach (var group in controlInfoDataGroup.Groups)
            {
                if (!Groups.Any(g => g.Title == group.Title))
                {
                    Groups.Add(group);
                }
            }
        }
    }

    private void UpdateItemInGroups(ObservableCollection<DataGroup> groups, bool autoIncludedInBuild)
    {
        foreach (var group in groups)
        {
            foreach (var item in group.Items)
            {
                UpdateItem(item, autoIncludedInBuild);
                UpdateItemInItems(item.Items, autoIncludedInBuild);
            }

            UpdateItemInItems(group.Items, autoIncludedInBuild);
        }
    }

    private void UpdateItemInItems(ObservableCollection<DataItem> items, bool autoIncludedInBuild)
    {
        foreach (var item in items)
        {
            UpdateItem(item, autoIncludedInBuild);
            UpdateItemInItems(item.Items, autoIncludedInBuild);
        }
    }

    private DataItem UpdateItem(DataItem item, bool autoIncludedInBuild)
    {
        string? badgeString = item switch
        {
            { IsNew: true } => "New",
            { IsUpdated: true } => "Updated",
            { IsPreview: true } => "Preview",
            _ => null
        };

        if (item.DataInfoBadge != null && item.DataInfoBadge.BadgeStyle == null)
        {
            item.DataInfoBadge.BadgeStyle = "AttentionValueInfoBadgeStyle";
        }

        item.BadgeString = badgeString;
        bool isIncludedInBuild = item.IncludedInBuild;
        string pageString = item.UniqueId;
        if (autoIncludedInBuild)
        {
            Type pageType = ApplicationHelper.GetPageType(pageString, item.ApiNamespace);
            

            isIncludedInBuild = pageType != null;
        }
        else
        {
            isIncludedInBuild = item.IncludedInBuild;
        }
        
        item.IncludedInBuild = isIncludedInBuild;
        return item;
    }
}
