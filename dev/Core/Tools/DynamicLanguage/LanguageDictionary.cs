using System.Diagnostics.CodeAnalysis;

namespace WindowUI;

public class LanguageDictionary
{
    public record Item(string Uid, string DependencyPropertyName, string Value, string StringResourceItemName);

    private readonly Dictionary<string, Items> dictionary = new();

    public LanguageDictionary(string language)
    {
        Language = language;
    }

    public string Language { get; }

    public void AddItem(Item item)
    {
        if (this.dictionary.ContainsKey(item.Uid) is true)
        {
            this.dictionary[item.Uid].Add(item);
        }
        else
        {
            this.dictionary[item.Uid] = new Items() { item };
        }
    }

    public IEnumerable<Item> GetItems()
    {
        return this.dictionary.Values.SelectMany(x => x).ToList();
    }

    public int GetItemsCount()
    {
        return this.dictionary.Values.Sum(x => x.Count);
    }

    public bool TryGetItems(string uid, [MaybeNullWhen(false)] out Items items)
    {
        return this.dictionary.TryGetValue(uid, out items);
    }

    public class Items : List<Item>, IEnumerable<Item> { }
}