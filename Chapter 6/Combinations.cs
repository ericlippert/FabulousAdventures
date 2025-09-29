// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 6

using System.Numerics;

static class Combinations
{
    public static void SampleCode()
    {

        Console.WriteLine("Listing 6.15 sample code");
        List<string> foods = ["ham", "jam", "spam", "lamb"];
        Console.WriteLine(foods.Combine([1, 2]).Space());

        Console.WriteLine("Listing 6.16 sample code");
        foreach (var comb in Combinations1(6, 4))
            Console.Write(comb.Concat() + " ");
        Console.WriteLine();


        Console.WriteLine("Listing 6.19 sample code");
        Console.WriteLine(20.Choose(10));


        Console.WriteLine("Listing 6.20 sample code");
        Console.WriteLine(GetCombination(8, 6, 4).Concat());

        Console.WriteLine("Listing 6.21 sample code");
        foreach (var c in Combinations2(6, 4))
            Console.Write($"{c.GetCombNumber(6, 4),2}:{c.Concat()} ");
        Console.WriteLine();

        Console.WriteLine("Listing 6.22 sample code");
        foreach (var c in Combinations1(6, 4))
            Console.Write($"{c.GetCombNumber(),2}:{c.Concat()} ");
        Console.WriteLine();


    }

    static bool NextLexiCombination(int n, IList<int> c)
    {
        int k = c.Count;

        bool Increasable(int i) =>
            i == k - 1 ? c[i] < k + 1 : c[i] + 1 < c[i + 1];

        int i = 0;
        while (i < k && !Increasable(i))
            i += 1;
        if (i == k)
            return false;
        c[i] += 1;
        for (int j = 0; j < i; j += 1)
            c[j] = j;
        return true;
    }
    static IEnumerable<IList<int>> Combinations1(int n, int k)
    {
        var c = Enumerable.Range(0, k).ToArray();
        do
            yield return c.ToList();
        while (NextLexiCombination(n, c));
    }
    static IEnumerable<IDeque<int>> Combinations2(int n, int k)
    {
        if (k > n || k < 0)
            yield break;
        if (k == 0)
        {
            yield return Deque<int>.Empty;
            yield break;
        }
        foreach (var r in Combinations2(n - 1, k))
            yield return r;
        foreach (var r in Combinations2(n - 1, k - 1))
            yield return r.PushRight(n - 1);
    }

    static IDeque<int> GetCombination(BigInteger p, int n, int k)
    {
        if (k > n || k == 0)
            return Deque<int>.Empty;
        var boundary = (n - 1).Choose(k);
        return p < boundary ?
            GetCombination(p, n - 1, k) :
            GetCombination(p - boundary, n - 1, k - 1).PushRight(n - 1);
    }
}


static partial class Extensions
{
    // Listing 6.14

    public static IList<T> Combine<T>(
        this IList<T> items, IList<int> combination)
    {
        var result = new T[combination.Count];
        for (int i = 0; i < combination.Count; i += 1)
            result[i] = items[combination[i]];
        return result;
    }


    // Listing 6.18
    public static BigInteger Choose(this int n, int k)
    {
        if (k > n) return 0;
        if (k > n / 2)
            k = n - k;
        BigInteger result = 1;
        for (int i = 1; i <= k; i += 1)
            result = result * (n - k + i) / i;  // Careful!
        return result;
    }


    // Listing 6.20
    public static BigInteger GetCombNumber(
        this IDeque<int> comb, int n, int k) =>
        comb.IsEmpty ?
            0 :
            comb.Right() == n - 1 ?
                (n - 1).Choose(k) + comb.PopRight().GetCombNumber(n - 1, k - 1) :
                comb.GetCombNumber(n - 1, k);

    // Listing 6.22
    public static BigInteger GetCombNumber(this IList<int> comb)
    {
        BigInteger result = 0;
        int k = comb.Count;
        for (int i = 0; i < k; i += 1)
            result += comb[i].Choose(i + 1);
        return result;
    }
}
