
// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 6

using System.Numerics;

static class Permutations
{
    public static void SampleCode()
    {

        Console.WriteLine("Listing 6.7 sample code");
        List<string> animals = ["cat", "dog", "elk", "fox", "hog"];
        Console.WriteLine(animals.Permute([1, 0, 2, 4, 3]).Space());

        Console.WriteLine("Listing 6.8 sample code");
        List<char> list = ['A', 'B', 'B', 'E', 'Y'];
        do
            Console.Write(list.Concat() + " ");
        while (list.NextLexiPermutation());
        Console.WriteLine();

        Console.WriteLine("Listing 6.9 sample code");
        Console.WriteLine(GetLexiPermutation(4, 9).Concat());

        Console.WriteLine("Listing 6.10 sample code");
        List<int> shuffleMe = [0, 1, 2, 3];
        shuffleMe.FYShuffle();
        Console.WriteLine(shuffleMe.Concat());

        Console.WriteLine("Listing 6.11 sample code");
        for (int p = 0; p < 24; p += 1)
            Console.Write($"{p,2}:{GetFYPermutation(4, p).Concat()} ");
        Console.WriteLine();

        Console.WriteLine("Listing 6.12 sample code");
        foreach (var change in Changes(4))
            Console.Write(change.Concat() + " ");
        Console.WriteLine();

        Console.WriteLine("Listing 6.13 sample code");
        Console.WriteLine(GetChange(4, 5).Concat());

        Console.WriteLine("Listing 6.14 sample code");
        foreach (var perm in EvensChanges(4))
            Console.Write(perm.Concat() + " ");
        Console.WriteLine();
    }

    static IEnumerable<int> GetLexiPermutation(int n, BigInteger p)
    {
        var remaining = new List<int>(Enumerable.Range(0, n));
        var result = new List<int>(n);
        var f = n.Factorial();
        for (int cur = n; cur > 0; cur -= 1)
        {
            f /= cur;
            var r = (int)(p / f);
            p %= f;
            result.Add(remaining[r]);
            remaining.RemoveAt(r);
        }
        return result;
    }

    static IList<int> GetFYPermutation(int n, BigInteger p)
    {
        var result = new List<int>(Enumerable.Range(0, n));
        var f = n.Factorial();
        for (int cur = n; cur > 0; cur -= 1)
        {
            f /= cur;
            var r = (int)(p / f);
            p %= f;
            result.Swap(n - cur, n - cur + r);
        }
        return result;
    }


    static IEnumerable<IEnumerable<int>> Changes(int n)
    {
        if (n == 0)
        {
            yield return [];
            yield break;
        }
        bool fromLeft = false;
        foreach (var perm in Changes(n - 1))
        {
            for (int row = 0; row < n; row += 1)
                yield return perm.InsertAt(fromLeft ? row : n - row - 1, n - 1);
            fromLeft = !fromLeft;
        }
    }

    static IEnumerable<int> GetChange(int n, BigInteger p)
    {
        if (n == 0)
            return [];
        BigInteger column = p / n;
        IEnumerable<int> perm = GetChange(n - 1, column);
        bool fromLeft = column % 2 != 0;
        int row = (int)(p % n);
        return perm.InsertAt(fromLeft ? row : n - row - 1, n - 1);
    }

    
    static IEnumerable<IList<int>> EvensChanges(int n)
    {
        var perm = Enumerable.Range(0, n).ToArray();
        // Right == +1, left == -1
        var dirs = Enumerable.Repeat(-1, n).ToArray();

        bool IsMobile(int i)
        {
            int j = i + dirs[perm[i]];
            return 0 <= j && j < n && perm[j] < perm[i];
        }

        int? MaxMobile()
        {
            int max = -1;
            int? maxIndex = null;
            for (int i = 0; i < perm.Length; i += 1)
            {
                if (IsMobile(i) && perm[i] > max)
                {
                    maxIndex = i;
                    max = perm[i];
                }
            }
            return maxIndex;
        }

        while (true)
        {
            yield return perm.ToList();
            // Find the index of the largest mobile number
            int? maxIndex = MaxMobile();
            if (maxIndex == null) // No more permutations
                yield break;
            int mi = maxIndex.Value;
            int m = perm[mi];
            // Move the max mobile
            perm.Swap(mi, mi + dirs[m]);
            // Update dirs
            for (int k = m + 1; k < n; k += 1)
                dirs[k] = -dirs[k];
        }
    }
}

static partial class Extensions
{
    // Listing 6.7
    public static IList<T> Permute<T>(this IList<T> items, IList<int> permutation)
    {
        if (items.Count != permutation.Count)
            throw new InvalidOperationException();
        var result = new T[items.Count];
        for (int i = 0; i < items.Count; i += 1)
            result[i] = items[permutation[i]];
        return result;
    }


    // Listing 6.8
    public static bool Greater<T>(this IList<T> items, int x, int y)
        where T : IComparable<T> =>
        items[x].CompareTo(items[y]) > 0;

    public static void Swap<T>(this IList<T> items, int x, int y) =>
        (items[x], items[y]) = (items[y], items[x]);

    public static void ReverseRange<T>(this IList<T> items, int start, int end)
    {
        for (; start < end; start += 1, end -= 1)
            items.Swap(start, end);
    }
    public static bool NextLexiPermutation<T>(this IList<T> items) where T : IComparable<T>
    {
        int first = items.Count - 2;
        while (first >= 0 && !items.Greater(first + 1, first))
            first -= 1;
        if (first < 0)
            return false; // There was no last pair
        int second = items.Count - 1;
        while (!items.Greater(second, first))
            second -= 1;
        items.Swap(first, second);
        items.ReverseRange(first + 1, items.Count - 1);
        return true;
    }

    // Listing 6.9
    public static BigInteger Factorial(this int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i += 1)
            result *= i;
        return result;
    }

    // Listing 6.10
    public static void FYShuffle<T>(this IList<T> items)
    {
        var r = new Random();
        for (int i = 0; i < items.Count - 1; i += 1)
            items.Swap(i, i + r.Next(0, items.Count - i));
    }

    // Listing 6.11
    public static IEnumerable<T> InsertAt<T>(this IEnumerable<T> items, int index, T insert)
    {
        int current = 0;
        foreach (T item in items)
        {
            if (current == index)
                yield return insert;
            yield return item;
            current += 1;
        }
        if (current == index)
            yield return insert;
    }
}
