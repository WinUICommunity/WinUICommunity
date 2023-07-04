using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;

using Microsoft.UI.Xaml;

namespace WinUICommunity;

public class ControlInfoDataSource
{
    private static readonly object _lock = new();

    public ObservableCollection<ControlInfoDataGroup> Groups { get; set; } = new ObservableCollection<ControlInfoDataGroup>();
    public async Task<IEnumerable<ControlInfoDataGroup>> GetGroupsAsync(string jsonFilePath, PathType pathType, IncludedInBuildMode includedInBuildMode = IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty)
    {
        await GetControlInfoDataAsync(jsonFilePath, pathType, includedInBuildMode);

        return Groups;
    }
    public async Task<ControlInfoDataGroup> GetGroupAsync(string uniqueId, string jsonFilePath, PathType pathType, IncludedInBuildMode includedInBuildMode = IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty)
    {
        await GetControlInfoDataAsync(jsonFilePath, pathType, includedInBuildMode);
        // Simple linear search is acceptable for small data sets
        var matches = Groups.Where((group) => group.UniqueId.Equals(uniqueId));
        return matches.Count() == 1 ? matches.First() : null;
    }

    public async Task<ControlInfoDataItem> GetItemAsync(string uniqueId, string jsonFilePath, PathType pathType, IncludedInBuildMode includedInBuildMode = IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty)
    {
        await GetControlInfoDataAsync(jsonFilePath, pathType, includedInBuildMode);
        // Simple linear search is acceptable for small data sets
        var matches = FindItems(Groups.SelectMany(group => group.Items), uniqueId).ToList();

        return matches.Count() > 0 ? matches.First() : null;
    }
    private IEnumerable<ControlInfoDataItem> FindItems(IEnumerable<ControlInfoDataItem> items, string uniqueId)
    {
        foreach (var item in items)
        {
            if (item.UniqueId.Equals(uniqueId))
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
    public async Task GetControlInfoDataAsync(string jsonFilePath, PathType pathType, IncludedInBuildMode includedInBuildMode = IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty)
    {
        lock (_lock)
        {
            if (this.Groups.Count() != 0)
            {
                return;
            }
        }

        string filePath = await FileLoader.GetPath(jsonFilePath, pathType);
        using FileStream openStream = File.OpenRead(filePath);
        var controlInfoDataGroup = await JsonSerializer.DeserializeAsync<ControlInfoDataSource>(openStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        lock (_lock)
        {
            UpdateItemInGroups(controlInfoDataGroup.Groups, includedInBuildMode);

            foreach (var group in controlInfoDataGroup.Groups)
            {
                if (!Groups.Any(g => g.Title == group.Title))
                {
                    Groups.Add(group);
                }
            }
        }
    }
    private void UpdateItemInGroups(ObservableCollection<ControlInfoDataGroup> groups, IncludedInBuildMode includedInBuildMode)
    {
        foreach (var group in groups)
        {
            foreach (var item in group.Items)
            {
                UpdateItem(item, includedInBuildMode);
                UpdateItemInItems(item.Items, includedInBuildMode);
            }

            UpdateItemInItems(group.Items, includedInBuildMode);
        }
    }
    private void UpdateItemInItems(ObservableCollection<ControlInfoDataItem> items, IncludedInBuildMode includedInBuildMode)
    {
        foreach (var item in items)
        {
            UpdateItem(item, includedInBuildMode);
            UpdateItemInItems(item.Items, includedInBuildMode);
        }
    }
    private ControlInfoDataItem UpdateItem(ControlInfoDataItem item, IncludedInBuildMode includedInBuildMode)
    {
        string? badgeString = item switch
        {
            { IsNew: true } => "New",
            { IsUpdated: true } => "Updated",
            { IsPreview: true } => "Preview",
            _ => null
        };

        if (item.InfoBadge != null && item.InfoBadge.BadgeStyle == null)
        {
            item.InfoBadge.BadgeStyle = "AttentionValueInfoBadgeStyle";
        }

        item.BadgeString = badgeString;
        bool isIncludedInBuild = item.IncludedInBuild;
        string pageString = item.UniqueId;
        if (includedInBuildMode == IncludedInBuildMode.RealCheckBasedOnUniqeIdPath)
        {
            Type pageType = null;
            Assembly assembly = string.IsNullOrEmpty(item.ApiNamespace) ? Application.Current.GetType().Assembly : Assembly.Load(item.ApiNamespace);
            if (assembly is not null)
            {
                pageType = assembly.GetType(pageString);
            }
            isIncludedInBuild = pageType != null;
        }
        else if (includedInBuildMode == IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty)
        {
            isIncludedInBuild = item.IncludedInBuild;
        }
        else
        {
            // Default to false if no valid IncludedInBuildMode provided
            isIncludedInBuild = false;
        }
        item.IncludedInBuild = isIncludedInBuild;
        return item;
    }
}
