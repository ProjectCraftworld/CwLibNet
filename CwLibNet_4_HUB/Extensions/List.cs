using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Extensions;

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