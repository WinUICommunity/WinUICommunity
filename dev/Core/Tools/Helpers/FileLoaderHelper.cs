namespace WinUICommunity;

public static class FileLoaderHelper
{
    public static async Task<string> GetPath(string filePath, PathType pathType = PathType.Relative)
    {
        StorageFile file = null;
        if (ApplicationHelper.IsPackaged)
        {
            switch (pathType)
            {
                case PathType.Relative:
                    var sourceUri = new Uri("ms-appx:///" + filePath);
                    file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);
                    break;
                case PathType.Absolute:
                    file = await StorageFile.GetFileFromPathAsync(filePath);
                    break;
            }
        }
        else
        {
            switch (pathType)
            {
                case PathType.Relative:
                    var sourcePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filePath));
                    file = await StorageFile.GetFileFromPathAsync(sourcePath);
                    break;
                case PathType.Absolute:
                    file = await StorageFile.GetFileFromPathAsync(filePath);
                    break;
            }
        }

        return file.Path;
    }
}
