using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
internal class PriResourceReaderFactory
{
    private readonly Dictionary<string, PriResourceReader> readers = new();

    internal PriResourceReader GetPriResourceReader(string? priFile)
    {
        string normalizedFilePath = string.Empty;

        if (string.IsNullOrEmpty(priFile) is false)
        {
            normalizedFilePath = System.IO.Path.GetFullPath(priFile);
        }

        if (this.readers.TryGetValue(normalizedFilePath, out PriResourceReader? reader) is false)
        {
            ResourceManager manager;

            if (string.IsNullOrEmpty(normalizedFilePath) is false)
            {
                manager = new ResourceManager(normalizedFilePath);
            }
            else
            {
                manager = new ResourceManager();
            }

            reader = new PriResourceReader(manager);
            this.readers[normalizedFilePath] = reader;
        }

        return reader;
    }
}
