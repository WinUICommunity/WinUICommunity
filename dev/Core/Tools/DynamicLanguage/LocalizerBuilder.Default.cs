namespace WindowUI;

public partial class LocalizerBuilder
{
    public static async Task CreateStringResourceFileIfNotExists(StorageFolder stringsFolder, string language, string resourceFileName)
    {
        StorageFolder languageFolder = await stringsFolder.CreateFolderAsync(
            language,
            CreationCollisionOption.OpenIfExists);

        if (await languageFolder.TryGetItemAsync(resourceFileName) is null)
        {
            string resourceFilePath = Path.Combine(stringsFolder.Name, language, resourceFileName);
            StorageFile resourceFile = await LoadStringResourcesFileFromAppResource(resourceFilePath);
            _ = await resourceFile.CopyAsync(languageFolder);
        }
    }

    private static async Task<StorageFile> LoadStringResourcesFileFromAppResource(string filePath)
    {
        Uri resourcesFileUri = new($"ms-appx:///{filePath}");
        return await StorageFile.GetFileFromApplicationUriAsync(resourcesFileUri);
    }
}
