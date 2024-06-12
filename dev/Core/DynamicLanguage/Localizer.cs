namespace WinUICommunity;

[Obsolete("This Feature will be removed after WASDK v1.6 released")]
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

        DependencyObjectUidSet += Uids_DependencyObjectUidSet;
    }

    public event EventHandler<LanguageChangedEventArgs>? LanguageChanged;

    private static ILocalizer Instance { get; set; } = NullLocalizer.Instance;

    private bool IsDisposed { get; set; }

    private LanguageDictionary CurrentDictionary { get; set; } = new("");

    public static ILocalizer Get() => Instance;

    public IEnumerable<string> GetAvailableLanguages()
    {
        try
        {
            return this.languageDictionaries
                .Values
                .Select(x => x.Language)
                .ToArray();
        }
        catch (Exception exception)
        {
            FailedToGetAvailableLanguagesException localizerException = new(innerException: exception);
            throw localizerException;
        }
    }

    public string GetCurrentLanguage() => CurrentDictionary.Language;

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
            FailedToSetLanguageException localizerException = new(previousLanguage, language, message: string.Empty, innerException: exception);
            throw localizerException;
        }
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
        catch (LocalizerException)
        {
            throw;
        }
        catch (Exception exception)
        {
            FailedToGetLocalizedStringException localizerException = new(uid, innerException: exception);
            throw localizerException;
        }

        return string.Empty;
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
        catch (LocalizerException)
        {
            throw;
        }
        catch (Exception exception)
        {
            FailedToGetLocalizedStringException localizerException = new(uid, innerException: exception);
            throw localizerException;
        }

        return Array.Empty<string>();
    }

    public LanguageDictionary GetCurrentLanguageDictionary() => CurrentDictionary;

    public IEnumerable<LanguageDictionary> GetLanguageDictionaries() => this.languageDictionaries.Values;

    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    internal static void Set(ILocalizer localizer) => Instance = localizer;

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
        if (type.GetField(
            dependencyPropertyName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is FieldInfo fieldInfo &&
            fieldInfo.GetValue(null) is DependencyProperty field)
        {
            return field;
        }

        if (dependencyPropertyName.Split(".") is string[] splitResult &&
            splitResult.Length is 2)
        {
            string attachedPropertyClassName = splitResult[0];
            IEnumerable<Type> types = GetTypesFromName(attachedPropertyClassName);

            string attachedPropertyName = splitResult[1];
            IEnumerable<PropertyInfo> attachedProperties = types
                .Select(x => x.GetProperty(attachedPropertyName))
                .OfType<PropertyInfo>();

            foreach (PropertyInfo attachedProperty in attachedProperties)
            {
                if (attachedProperty.GetValue(null) is DependencyProperty dependencyProperty)
                {
                    return dependencyProperty;
                }
            }
        }

        return null;
    }

    private static IEnumerable<Type> GetTypesFromName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.Name == name);
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
                LocalizeDependencyObject(dependencyObject, item);
            }
        }
    }

    private void LocalizeDependencyObject(DependencyObject dependencyObject, LanguageDictionary.Item item)
    {
        if (GetDependencyProperty(
            dependencyObject,
            item.DependencyPropertyName) is DependencyProperty dependencyProperty)
        {
            LocalizeDependencyObjectsWithDependencyProperty(dependencyObject, dependencyProperty, item.Value);
            return;
        }
        LocalizeDependencyObjectsWithoutDependencyProperty(dependencyObject, item.Value);
    }

    private void LocalizeDependencyObjectsWithDependencyProperty(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string value)
    {
        if (dependencyObject
            .GetValue(dependencyProperty)?
            .GetType() is Type propertyType &&
            propertyType.IsEnum is true &&
            Enum.TryParse(propertyType, value, out object? enumValue) is true)
        {
            dependencyObject.SetValue(dependencyProperty, enumValue);
            return;
        }

        dependencyObject.SetValue(dependencyProperty, value);
    }

    private void LocalizeDependencyObjectsWithoutDependencyProperty(DependencyObject dependencyObject, string value)
    {
        foreach (LocalizationActions.ActionItem item in this.localizationActions
            .Where(x => x.TargetType == dependencyObject.GetType()))
        {
            item.Action(new LocalizationActions.ActionArguments(dependencyObject, value));
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
