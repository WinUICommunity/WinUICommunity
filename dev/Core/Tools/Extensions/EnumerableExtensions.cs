namespace WindowUI;
public struct EnumeratorWithIndex<T>
{
    public readonly T Value;
    public readonly int Index;

    public EnumeratorWithIndex(T value, int index)
    {
        this.Value = value;
        this.Index = index;
    }

    public static EnumeratorWithIndex<T> Create(T value, int index)
    {
        return new EnumeratorWithIndex<T>(value, index);
    }
}
public static class EnumerableExtensions
{
    /// <summary>
    /// This Extension Help to access item index in foreach loop
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static IEnumerable<EnumeratorWithIndex<T>> GetEnumeratorWithIndex<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Select(EnumeratorWithIndex<T>.Create);
    }
}
