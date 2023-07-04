using System.Xml;

namespace WinUICommunity;

public partial class LocalizerBuilder
{
    private record StringResourceItem(string Name, string Value, string Comment);

    private record StringResourceItems(string Language, IEnumerable<StringResourceItem> Items);

    private readonly List<Action> builderActions = new();

    private readonly List<LanguageDictionary> languageDictionaries = new();

    private readonly List<LocalizationActions.ActionItem> localizationActions = new();

    private readonly Localizer.Options options = new();

    public static string StringResourcesFileXPath { get; set; } = "//root/data";

    public static bool IsLocalizerAlreadyBuilt => Localizer.Get() is Localizer;

    public LocalizerBuilder AddStringResourcesFolderForLanguageDictionaries(
        string stringResourcesFolderPath,
        string resourcesFileName = "Resources.resw",
        bool ignoreExceptions = false)
    {
        this.builderActions.Add(() =>
        {
            foreach (string languageFolderPath in Directory.GetDirectories(stringResourcesFolderPath))
            {
                try
                {
                    string languageFilePath = Path.Combine(languageFolderPath, resourcesFileName);

                    if (CreateLanguageDictionaryFromStringResourcesFile(
                        languageFilePath,
                        StringResourcesFileXPath) is LanguageDictionary dictionary)
                    {
                        this.languageDictionaries.Add(dictionary);
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
            throw new LocalizerException($"{nameof(Localizer)} is already built.");
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

    private static LanguageDictionary? CreateLanguageDictionaryFromStringResourcesFile(string filePath, string fileXPath)
    {
        return CreateStringResourceItemsFromResourcesFile(
            filePath,
            fileXPath) is StringResourceItems stringResourceItems
            ? CreateLanguageDictionaryFromStringResourceItems(stringResourceItems)
            : null;
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
        (string Uid, string DependencyPropertyName) = name.LastIndexOf(".") is int lastSeparatorIndex && lastSeparatorIndex > 1
            ? (name[..lastSeparatorIndex], string.Concat(name.AsSpan(lastSeparatorIndex + 1), "Property"))
            : (name, string.Empty);
        return new LanguageDictionary.Item(
            Uid,
            DependencyPropertyName,
            stringResourceItem.Value,
            stringResourceItem.Name);
    }

    private static StringResourceItems? CreateStringResourceItemsFromResourcesFile(string filePath, string xPath = "//root/data")
    {
        DirectoryInfo directoryInfo = new(filePath);

        if (directoryInfo.Parent?.Name is string language)
        {
            XmlDocument document = new();
            if (File.Exists(directoryInfo.FullName))
            {
                document.Load(directoryInfo.FullName);

                if (document.SelectNodes(xPath) is XmlNodeList nodeList)
                {
                    List<StringResourceItem> items = new();
                    items.AddRange(CreateStringResourceItems(nodeList));
                    return new StringResourceItems(language, items);
                }
            }
        }
        return null;
    }

    private static IEnumerable<StringResourceItem> CreateStringResourceItems(XmlNodeList nodeList)
    {
        foreach (XmlNode node in nodeList)
        {
            if (CreateStringResourceItem(node) is StringResourceItem item)
            {
                yield return item;
            }
        }
    }

    private static StringResourceItem? CreateStringResourceItem(XmlNode node)
    {
        return new StringResourceItem(
            Name: node.Attributes?["name"]?.Value ?? string.Empty,
            Value: node["value"]?.InnerText ?? string.Empty,
            Comment: string.Empty);
    }
}