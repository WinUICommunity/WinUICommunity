using System.Xml;

namespace WindowUI;

public partial class LocalizerBuilder
{
    private record StringResourceItem(string Name, string Value, string Comment);

    private record StringResourceItems(string Language, IEnumerable<StringResourceItem> Items);

    private readonly List<Action> builderActions = new();

    private readonly List<LanguageDictionary> languageDictionaries = new();

    private readonly List<LocalizationActions.ActionItem> localizationActions = new();

    private readonly Localizer.Options options = new();

    private string defaultStringResourcesFileName = "Resources.resw";

    private string stringResourcesFileXPath = "//root/data";

    public static bool IsLocalizerAlreadyBuilt => Localizer.Get() is Localizer;

    public LocalizerBuilder SetDefaultStringResourcesFileName(string fileName)
    {
        this.defaultStringResourcesFileName = fileName;
        return this;
    }

    public LocalizerBuilder SetStringResourcesFileXPath(string xPath)
    {
        this.stringResourcesFileXPath = xPath;
        return this;
    }

    public LocalizerBuilder AddStringResourcesFolderForLanguageDictionaries(
        string stringResourcesFolderPath,
        bool ignoreExceptions = false)
    {
        this.builderActions.Add(() =>
        {
            foreach (string languageFolderPath in Directory.GetDirectories(stringResourcesFolderPath))
            {
                try
                {
                    foreach (string stringResourcesFileFullPath in Directory.GetFiles(languageFolderPath, "*.resw"))
                    {
                        string fileName = Path.GetFileName(stringResourcesFileFullPath);
                        string sourceName = fileName == this.defaultStringResourcesFileName
                            ? string.Empty
                            : Path.GetFileNameWithoutExtension(fileName);

                        if (CreateLanguageDictionaryFromStringResourcesFile(
                            sourceName,
                            stringResourcesFileFullPath,
                            this.stringResourcesFileXPath) is LanguageDictionary dictionary)
                        {
                            this.languageDictionaries.Add(dictionary);
                        }
                    }
                }
                catch
                {
                    if (ignoreExceptions is false)
                    {
                        throw;
                    }
                }
            }
        });

        return this;
    }

    public LocalizerBuilder AddLanguageDictionary(LanguageDictionary dictionary)
    {
        this.builderActions.Add(() => this.languageDictionaries.Add(dictionary));
        return this;
    }

    public LocalizerBuilder AddLocalizationAction(LocalizationActions.ActionItem item)
    {
        this.localizationActions.Add(item);
        return this;
    }

    public LocalizerBuilder SetOptions(Action<Localizer.Options>? options)
    {
        options?.Invoke(this.options);
        return this;
    }

    public async Task<ILocalizer> Build()
    {
        if (IsLocalizerAlreadyBuilt is true)
        {
            LocalizerIsAlreadyBuiltException localizerException = new();
            throw localizerException;
        }

        Localizer localizer = new(this.options);

        foreach (Action action in this.builderActions)
        {
            action.Invoke();
        }

        foreach (LanguageDictionary dictionary in this.languageDictionaries)
        {
            localizer.AddLanguageDictionary(dictionary);
        }

        foreach (LocalizationActions.ActionItem item in this.localizationActions)
        {
            localizer.AddLocalizationAction(item);
        }

        await localizer.SetLanguage(this.options.DefaultLanguage);

        Localizer.Set(localizer);
        return localizer;
    }

    private static LanguageDictionary? CreateLanguageDictionaryFromStringResourcesFile(string sourceName, string filePath, string fileXPath)
    {
        if (CreateStringResourceItemsFromResourcesFile(
            sourceName,
            filePath,
            fileXPath) is StringResourceItems stringResourceItems)
        {
            return CreateLanguageDictionaryFromStringResourceItems(stringResourceItems);
        }

        return null;
    }

    private static LanguageDictionary CreateLanguageDictionaryFromStringResourceItems(StringResourceItems stringResourceItems)
    {
        LanguageDictionary dictionary = new(stringResourceItems.Language);

        foreach (StringResourceItem stringResourceItem in stringResourceItems.Items)
        {
            LanguageDictionary.Item item = CreateLanguageDictionaryItem(stringResourceItem);
            dictionary.AddItem(item);
        }

        return dictionary;
    }

    private static LanguageDictionary.Item CreateLanguageDictionaryItem(StringResourceItem stringResourceItem)
    {
        string name = stringResourceItem.Name;
        (string Uid, string DependencyPropertyName) = name.IndexOf(".") is int firstSeparatorIndex && firstSeparatorIndex > 1
            ? (name[..firstSeparatorIndex], string.Concat(name.AsSpan(firstSeparatorIndex + 1), "Property"))
            : (name, string.Empty);
        return new LanguageDictionary.Item(
            Uid,
            DependencyPropertyName,
            stringResourceItem.Value,
            stringResourceItem.Name);
    }

    private static StringResourceItems? CreateStringResourceItemsFromResourcesFile(string sourceName, string filePath, string xPath = "//root/data")
    {
        DirectoryInfo directoryInfo = new(filePath);

        if (directoryInfo.Parent?.Name is string language)
        {
            XmlDocument document = new();
            document.Load(directoryInfo.FullName);

            if (document.SelectNodes(xPath) is XmlNodeList nodeList)
            {
                List<StringResourceItem> items = new();
                IEnumerable<StringResourceItem> stringResourceItems = CreateStringResourceItems(sourceName, nodeList);
                items.AddRange(stringResourceItems);
                return new StringResourceItems(language, items);
            }
        }

        return null;
    }

    private static IEnumerable<StringResourceItem> CreateStringResourceItems(string sourceName, XmlNodeList nodeList)
    {
        foreach (XmlNode node in nodeList)
        {
            if (CreateStringResourceItem(sourceName, node) is StringResourceItem item)
            {
                yield return item;
            }
        }
    }

    private static StringResourceItem? CreateStringResourceItem(string sourceName, XmlNode node)
    {
        string prefix = string.IsNullOrEmpty(sourceName) is false
            ? $"/{sourceName}/"
            : string.Empty;

        return new StringResourceItem(
            Name: $"{prefix}{node.Attributes?["name"]?.Value ?? string.Empty}",
            Value: node["value"]?.InnerText ?? string.Empty,
            Comment: string.Empty);
    }
}
