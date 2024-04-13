using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
internal class PriResourceReader
{
    private readonly ResourceManager resourceManager;

    internal PriResourceReader(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }

    public IEnumerable<LanguageDictionary.Item> GetItems(string language, string subTreeName = "Resources")
    {
        if (string.IsNullOrEmpty(subTreeName) is true ||
            subTreeName == "/")
        {
            subTreeName = "Resources";
        }
        else if (subTreeName.EndsWith('/') is true)
        {
            subTreeName = subTreeName[..^1];
        }

        ResourceMap resourceMap = this.resourceManager.MainResourceMap.TryGetSubtree(subTreeName);

        if (resourceMap is not null)
        {
            ResourceContext resourceContext = this.resourceManager.CreateResourceContext();
            resourceContext.QualifierValues[KnownResourceQualifierName.Language] = language;

            return PriResourceReader.GetItemsCore(resourceMap, subTreeName, resourceContext);
        }

        return Enumerable.Empty<LanguageDictionary.Item>();
    }

    private static IEnumerable<LanguageDictionary.Item> GetItemsCore(ResourceMap resourceMap, string subTreeName, ResourceContext resourceContext)
    {
        bool isResourcesSubTree = string.Equals(subTreeName, "Resources", StringComparison.OrdinalIgnoreCase);
        uint count = resourceMap.ResourceCount;

        for (uint i = 0; i < count; i++)
        {
            (string key, ResourceCandidate? candidate) = resourceMap.GetValueByIndex(i, resourceContext);

            if (candidate is not null &&
                candidate.Kind is ResourceCandidateKind.String)
            {
                key = key.Replace('/', '.');

                if (!isResourcesSubTree)
                {
                    key = $"/{subTreeName}/{key}";
                }

                yield return LocalizerBuilder.CreateLanguageDictionaryItem(key, candidate.ValueAsString);
            }
        }
    }
}
