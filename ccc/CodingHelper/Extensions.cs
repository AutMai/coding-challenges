namespace CodingHelper;

public static class Extensions {
    public static int ToInt(this string s) => Convert.ToInt32(s);

    public static List<TSource> TakeAndRemove<TSource>(this List<TSource> source, int count) {
        var range = source.Take(count).ToList();
        source.RemoveRange(0, count);
        return range.ToList();
    }

    public static List<int> ToInt(this List<string> list) => list.Select(int.Parse).ToList();

    public static int IndexOfMin(this IList<int> self) {
        if (self == null) {
            throw new ArgumentNullException("self");
        }

        if (self.Count == 0) {
            throw new ArgumentException("List is empty.", "self");
        }

        int min = self[0];
        int minIndex = 0;

        for (int i = 1; i < self.Count; ++i) {
            if (self[i] < min) {
                min = self[i];
                minIndex = i;
            }
        }

        return minIndex;
    }
}