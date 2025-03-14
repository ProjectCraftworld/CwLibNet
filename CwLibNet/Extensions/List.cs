namespace CwLibNet.Extensions;

public static class ListExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var v in items)
        {
            collection.Add(v);
        }
    }
}