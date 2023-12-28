using Windows.Storage.Pickers;
using WinRT.Interop;

namespace WinUICommunity;
public partial class FileAndFolderPickerHelper
{
    private static FileSavePicker FileSavePicker(Window window, IDictionary<string, IList<string>> fileTypeChoices, string suggestedFileName, string defaultFileExtension, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder)
    {
        var savePicker = new FileSavePicker();
        savePicker.SuggestedStartLocation = suggestedStartLocation;
        foreach (var item in fileTypeChoices)
        {
            savePicker.FileTypeChoices.Add(item.Key, item.Value);
        }

        if (!string.IsNullOrEmpty(suggestedFileName))
        {
            savePicker.SuggestedFileName = suggestedFileName;
        }

        if (!string.IsNullOrEmpty(defaultFileExtension))
        {
            savePicker.DefaultFileExtension = defaultFileExtension;
        }

        InitializeWithWindow.Initialize(savePicker, WindowNative.GetWindowHandle(window));

        return savePicker;
    }

    private static FileOpenPicker FileOpenPicker(Window window, IList<string> fileTypeFilter, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail)
    {
        var picker = new FileOpenPicker();
        picker.ViewMode = pickerViewMode;
        picker.SuggestedStartLocation = suggestedStartLocation;
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(window));
        foreach (var item in fileTypeFilter)
        {
            picker.FileTypeFilter.Add(item);
        }

        return picker;
    }

    private static FolderPicker FolderPicker(Window window, IList<string> fileTypeFilter = null, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail)
    {
        FolderPicker folderPicker = new();
        InitializeWithWindow.Initialize(folderPicker, WindowNative.GetWindowHandle(window));

        if (fileTypeFilter != null)
        {
            foreach (var item in fileTypeFilter)
            {
                folderPicker.FileTypeFilter.Add(item);
            }
        }
        else
        {
            folderPicker.FileTypeFilter.Add("*");
        }

        folderPicker.SuggestedStartLocation = suggestedStartLocation;
        folderPicker.ViewMode = pickerViewMode;
        return folderPicker;
    }

    public static async Task<StorageFile> PickSaveFileAsync(Window window, IDictionary<string, IList<string>> fileTypeChoices, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder)
    {
        return await FileSavePicker(window, fileTypeChoices, null, null, suggestedStartLocation).PickSaveFileAsync();
    }

    public static async Task<StorageFile> PickSaveFileAsync(Window window, IDictionary<string, IList<string>> fileTypeChoices, string suggestedFileName, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder)
    {
        return await FileSavePicker(window, fileTypeChoices, suggestedFileName, null, suggestedStartLocation).PickSaveFileAsync();
    }

    public static async Task<StorageFile> PickSaveFileAsync(Window window, IDictionary<string, IList<string>> fileTypeChoices, string suggestedFileName, string defaultFileExtension, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder)
    {
        return await FileSavePicker(window, fileTypeChoices, suggestedFileName, defaultFileExtension, suggestedStartLocation).PickSaveFileAsync();
    }

    public static async Task<IReadOnlyList<StorageFile>> PickMultipleFilesAsync(Window window, IList<string> fileTypeFilter, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail)
    {
        return await FileOpenPicker(window, fileTypeFilter, suggestedStartLocation, pickerViewMode).PickMultipleFilesAsync();
    }

    public static async Task<StorageFile> PickSingleFileAsync(Window window, IList<string> fileTypeFilter, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail)
    {
        return await FileOpenPicker(window, fileTypeFilter, suggestedStartLocation, pickerViewMode).PickSingleFileAsync();
    }

    public static async Task<StorageFolder> PickSingleFolderAsync(Window window, IList<string> fileTypeFilter = null, PickerLocationId suggestedStartLocation = PickerLocationId.ComputerFolder, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail)
    {
        return await FolderPicker(window, fileTypeFilter, suggestedStartLocation, pickerViewMode).PickSingleFolderAsync();
    }
}
