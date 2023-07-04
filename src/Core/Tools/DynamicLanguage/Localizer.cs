namespace WinUICommunity;

public sealed partial class Localizer : ILocalizer, IDisposable
{
    public static readonly DependencyProperty UidProperty = DependencyProperty.RegisterAttached(
        "Uid",
        typeof(string),
        typeof(Localizer),
        new PropertyMetadata(default));

    /// <summary>
    /// This static event is meant only for the Localizer 
    /// so it can have access to DependencyObjects with Uid.
    /// </summary>
    internal static event EventHandler<DependencyObject>? DependencyObjectUidSet;

    public static string GetUid(DependencyObject dependencyObject)
    {
        return (string)dependencyObject.GetValue(UidProperty);
    }

    public static void SetUid(DependencyObject dependencyObject, string uid)
    {
        dependencyObject.SetValue(UidProperty, uid);
        DependencyObjectUidSet?.Invoke(null, dependencyObject);
    }

    private readonly Options options;

    private readonly DependencyObjectWeakReferences dependencyObjectsReferences = new();

    private readonly Dictionary<string, LanguageDictionary> languageDictionaries = new();

    private readonly List<LocalizationActions.ActionItem> localizationActions = new();

    internal Localizer(Options options)
    {
        this.options = options;

        if (this.options.DisableDefaultLocalizationActions is false)
        {
            this.localizationActions = LocalizationActions.DefaultActions;
        }

        DependencyObjectUidSet += Uids_DependencyObjectUidSet; ;
    }

    public event EventHandler<LanguageChangedEventArgs>? LanguageChanged;

    private static ILocalizer Instance { get; set; } = NullLocalizer.Instance;

    private bool IsDisposed { get; set; }

    private LanguageDictionary CurrentDictionary { get; set; } = new("");

    public static ILocalizer Get()
    {
        return Instance;
    }

    public IEnumerable<string> GetAvailableLanguages()
    {
        try
        {
            return this.languageDictionaries
                .Values
                .Select(x => x.Language)
                .ToArray();
        }
        catch (LocalizerException)
        {
            throw;
        }
        catch (Exception exception)
        {
            ThrowLocalizerException(exception, "Failed to get available languages.");
        }

        return Array.Empty<string>();
    }

    public string GetCurrentLanguage()
    {
        return CurrentDictionary.Language;
    }

    public async Task SetLanguage(string language)
    {
        string previousLanguage = CurrentDictionary.Language;

        try
        {
            if (this.languageDictionaries.TryGetValue(
                language,
                out LanguageDictionary? dictionary) is true &&
                dictionary is not null)
            {
                CurrentDictionary = dictionary;
                await LocalizeDependencyObjects();
                OnLanguageChanged(previousLanguage, CurrentDictionary.Language);
                return;
            }
        }
        catch (LocalizerException)
        {
            throw;
        }
        catch (Exception exception)
        {
            ThrowLocalizerException(exception, "Exception setting language. [{PreviousLanguage} -> {NextLanguage}]", previousLanguage, language);
        }

        ThrowLocalizerException(innerException: null, "Failed to set language. [{PreviousLanguage} -> {NextLanguage}]", previousLanguage, language);
        return;
    }

    public string GetLocalizedString(string uid)
    {
        try
        {
            if (this.languageDictionaries.TryGetValue(
                GetCurrentLanguage(),
                out LanguageDictionary? dictionary) is true &&
                dictionary?.TryGetItems(
                    uid,
                    out LanguageDictionary.Items? items) is true &&
                    items.LastOrDefault() is LanguageDictionary.Item item)
            {
                return item.Value;
            }
        }
        catch (Exception exception)
        {
            ThrowLocalizerException(exception, "Failed to get localized string. [Uid: {Uid}]", uid);
        }

        return this.options.UseUidWhenLocalizedStringNotFound is true
            ? uid
            : string.Empty;
    }

    public IEnumerable<string> GetLocalizedStrings(string uid)
    {
        try
        {
            if (this.languageDictionaries.TryGetValue(
                GetCurrentLanguage(),
                out LanguageDictionary? dictionary) is true &&
                dictionary?.TryGetItems(
                    uid,
                    out LanguageDictionary.Items? items) is true)
            {
                return items.Select(x => x.Value);
            }
        }
        catch (Exception exception)
        {
            ThrowLocalizerException(exception, "Failed to get localized string. [Uid: {Uid}]", uid);
        }

        return this.options.UseUidWhenLocalizedStringNotFound is true
            ? new string[] { uid }
            : Array.Empty<string>();
    }

    public LanguageDictionary GetCurrentLanguageDictionary()
    {
        return CurrentDictionary;
    }

    public IEnumerable<LanguageDictionary> GetLanguageDictionaries()
    {
        return this.languageDictionaries.Values;
    }

    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    internal static void Set(ILocalizer localizer)
    {
        Instance = localizer;
    }

    internal void AddLanguageDictionary(LanguageDictionary languageDictionary)
    {
        if (this.languageDictionaries.TryGetValue(
            languageDictionary.Language,
            out LanguageDictionary? targetDictionary) is true)
        {
            int previousItemsCount = targetDictionary.GetItemsCount();

            foreach (LanguageDictionary.Item item in languageDictionary.GetItems())
            {
                targetDictionary.AddItem(item);
            }

            return;
        }

        LanguageDictionary newDictionary = new(languageDictionary.Language);

        foreach (LanguageDictionary.Item item in languageDictionary.GetItems())
        {
            newDictionary.AddItem(item);
        }

        this.languageDictionaries.Add(newDictionary.Language, newDictionary);
    }

    internal void AddLocalizationAction(LocalizationActions.ActionItem item)
    {
        this.localizationActions.Add(item);
    }

    internal void RegisterDependencyObject(DependencyObject dependencyObject)
    {
        this.dependencyObjectsReferences.Add(dependencyObject);
        LocalizeDependencyObject(dependencyObject);
    }

    private static void Uids_DependencyObjectUidSet(object? sender, DependencyObject dependencyObject)
    {
        (Localizer.Instance as Localizer)?.RegisterDependencyObject(dependencyObject);
    }

    private static void ThrowLocalizerException(Exception? innerException, string message, params object?[] args)
    {
        LocalizerException localizerException = new(message, innerException);
        throw localizerException;
    }

    private static DependencyProperty? GetDependencyProperty(DependencyObject dependencyObject, string dependencyPropertyName)
    {
        Type type = dependencyObject.GetType();

        if (type.GetProperty(
            dependencyPropertyName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is PropertyInfo propertyInfo &&
            propertyInfo.GetValue(null) is DependencyProperty property)
        {
            return property;
        }
        else if (type.GetField(
            dependencyPropertyName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is FieldInfo fieldInfo &&
            fieldInfo.GetValue(null) is DependencyProperty field)
        {
            return field;
        }

        return null;
    }

    private void LocalizeDependencyObjectsWithoutDependencyProperty(DependencyObject dependencyObject, string value)
    {
        foreach (LocalizationActions.ActionItem item in this.localizationActions
            .Where(x => x.TargetType == dependencyObject.GetType()))
        {
            item.Action(new LocalizationActions.ActionArguments(dependencyObject, value));
        }
    }
    private async Task LocalizeDependencyObjects()
    {
        foreach (DependencyObject dependencyObject in await this.dependencyObjectsReferences.GetDependencyObjects())
        {
            LocalizeDependencyObject(dependencyObject);
        }
    }

    private void LocalizeDependencyObject(DependencyObject dependencyObject)
    {
        if (GetUid(dependencyObject) is string uid &&
            CurrentDictionary.TryGetItems(uid, out LanguageDictionary.Items? items) is true)
        {
            foreach (LanguageDictionary.Item item in items)
            {
                if (GetDependencyProperty(
                    dependencyObject,
                    item.DependencyPropertyName) is DependencyProperty dependencyProperty)
                {
                    dependencyObject.SetValue(dependencyProperty, item.Value);
                }
                else
                {
                    LocalizeDependencyObjectsWithoutDependencyProperty(dependencyObject, item.Value);
                }
            }
        }
    }

    private void OnLanguageChanged(string previousLanguage, string currentLanguage)
    {
        LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(previousLanguage, currentLanguage));
    }

    private void Dispose(bool isDisposing)
    {
        if (IsDisposed is not true && isDisposing is true)
        {
            this.dependencyObjectsReferences.Dispose();
            IsDisposed = true;
        }
    }
}