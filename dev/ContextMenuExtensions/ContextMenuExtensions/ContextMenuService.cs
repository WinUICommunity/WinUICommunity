namespace WinUICommunity;

public class ContextMenuService
{
    private const string MenusFolderName = "custom_commands";

    public static readonly ContextMenuService Ins = new();

    public async Task<List<ContextMenuItem>> QueryAllAsync()
    {
        var configFolder = await GetMenusFolderAsync();
        var files = await configFolder.GetFilesAsync();
        var result = new List<ContextMenuItem>(files.Count);
        foreach (var file in files)
        {
            var content = await FileIO.ReadTextAsync(file);
            try
            {
                var item = ContextMenuItem.ReadFromJson(content);
                item.File = file;
                result.Add(item);
            }
            catch (Exception)
            {
                var item = new ContextMenuItem
                {
                    Title = "<Error> config",
                    File = file
                };
                result.Add(item);
            }
        }
        result.Sort((l, r) => l.Index - r.Index);
        return result;
    }

    private async Task<StorageFile> CreateMenuFileAsync(string name)
    {
        var folder = await GetMenusFolderAsync();
        return await folder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);
    }

    public async Task<StorageFolder> GetMenusFolderAsync()
    {
        var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(MenusFolderName);
        if (item is StorageFile)
        {
            await item.DeleteAsync();
        }
        return await ApplicationData.Current.LocalFolder.CreateFolderAsync(MenusFolderName, CreationCollisionOption.OpenIfExists);
    }

    public async Task SaveAsync(ContextMenuItem item)
    {
        if (null == item)
        {
            throw new ArgumentNullException();
        }

        var (result, message) = ContextMenuItem.Check(item);
        if (!result)
        {
            throw new Exception($"{message} is empty");
        }

        if (item.File == null)
        {
            var fileName = item.Title + ".json";
            item.File = await CreateMenuFileAsync(fileName);
        }

        var content = ContextMenuItem.WriteToJson(item);
        await FileIO.WriteTextAsync(item.File, content);
    }


    public async Task DeleteAsync(ContextMenuItem item)
    {
        if (null == item)
        {
            throw new ArgumentNullException();
        }

        if (item.File == null)
        {
            return;
        }
        await item.File.DeleteAsync();
    }

    public async Task BuildToCacheAsync()
    {
        var configFolder = await GetMenusFolderAsync();
        var files = await configFolder.GetFilesAsync();

        var menus = ApplicationData.Current.LocalSettings.CreateContainer("menus", ApplicationDataCreateDisposition.Always).Values;
        menus.Clear();

        for (int i = 0; i < files.Count; i++)
        {
            var content = await FileIO.ReadTextAsync(files[i]);
            menus[i.ToString()] = content;
        }
    }

    public void ClearCache()
    {
        var menus = ApplicationData.Current.LocalSettings.CreateContainer("menus", ApplicationDataCreateDisposition.Always).Values;
        menus.Clear();
    }

    public async void OpenMenusFolderAsync()
    {
        var folder = await GetMenusFolderAsync();
        _ = await Launcher.LaunchFolderAsync(folder);
    }

    public async void OpenMenuFileAsync(ContextMenuItem item)
    {
        if (item.File == null)
        {
            return;
        }
        _ = await Launcher.LaunchFileAsync(item.File);
    }

    public string GetCustomMenuName()
    {
        var value = ApplicationData.Current.LocalSettings.Values["Custom_Menu_Name"];
        return (value as string) ?? "Open With";
    }

    public async void SetCustomMenuName(string name)
    {
        await Task.Run(() =>
        {
            ApplicationData.Current.LocalSettings.Values["Custom_Menu_Name"] = name ?? "Open With";
        });
    }

    public async void ClearAllMenus()
    {
        var folder = await GetMenusFolderAsync();
        await folder.DeleteAsync();
    }
}
