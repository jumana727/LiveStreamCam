using System.Collections;

namespace ApplicationCore.Extensions;

public static class ListExtensions
{
    public static void AddRangeUnique(this IList self, IEnumerable items)
    {
        foreach (var item in items)
            if (!self.Contains(item)) self.Add(item);
    }
}
