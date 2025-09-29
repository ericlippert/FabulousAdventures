// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 6

using System.Text;

static class CartesianProduct
{
    public static void SampleCode()
    {
        Console.WriteLine("Listing 6.1 sample code");
        HashSet<CharacterClass> cs =
          [new Wizard(), new Warrior(), new Thief()];
        HashSet<Weapon> ws =
          [new Longsword(), new Wand(), new Dagger()];

        var sb = new StringBuilder();
        foreach (var c in cs)
            foreach (var w in ws)
                sb.AppendLine($"{c.Name} {w.Name} {c.CanWield(w)}");
        var s = sb.ToString();
        Console.WriteLine(s);

        var q = from c in cs
                from w in ws
                select $"{c.Name} {w.Name} {c.CanWield(w)}";
        s = string.Join("\n", q);
        Console.WriteLine(s);

        HashSet<Monster> ms =
          [new Vampire(), new Werewolf()];
        foreach (var t in cs.CartesianProduct(ws).CartesianProduct(ms))
            Console.WriteLine($"{t.a.a} {t.a.b} {t.b}");

        Console.WriteLine("Listing 6.2 sample code");
        foreach (var t in cs.CartesianProduct(ws, ms))
            Console.WriteLine($"{t.a} {t.b} {t.c}");

        Console.WriteLine("Listing 6.4 sample code");
        s = cs.CartesianProduct(ws,
               (c, w) => $"{c.Name} {w.Name} {c.CanWield(w)}")
              .Newlines();
        Console.WriteLine(s);

        Console.WriteLine("Listing 6.6 sample code");
        List<string> items = ["ABC", "DEF", "GH"];
        foreach (var seq in items.CartesianProduct())
            Console.Write(seq.Concat() + " ");
        Console.WriteLine();
    }
}

static partial class Extensions
{
    // Listing 6.1
    public static HashSet<(A a, B b)> CartesianProduct<A, B>(
        this HashSet<A> hsa, HashSet<B> hsb)
    {
        var result = new HashSet<(A a, B b)>();
        foreach (var a in hsa)
            foreach (var b in hsb)
                result.Add((a, b));
        return result;
    }

    // Listing 6.2
    public static HashSet<(A a, B b, C c)> CartesianProduct<A, B, C>(
        this HashSet<A> hsa, HashSet<B> hsb, HashSet<C> hsc)
    {
        var result = new HashSet<(A a, B b, C c)>();
        foreach (var a in hsa)
            foreach (var b in hsb)
                foreach (var c in hsc)
                    result.Add((a, b, c));
        return result;
    }

    // Listing 6.3
    public static IEnumerable<(A a, B b)> CartesianProduct<A, B>(
        this IEnumerable<A> seqa, IEnumerable<B> seqb)
    {
        foreach (var a in seqa)
            foreach (var b in seqb)
                yield return (a, b);
    }

    // Listing 6.4
    public static IEnumerable<R> CartesianProduct<A, B, R>(
    this IEnumerable<A> seqa, IEnumerable<B> seqb, Func<A, B, R> projection)
    {
        foreach (var a in seqa)
            foreach (var b in seqb)
                yield return projection(a, b);
    }

    // Listing 6.5

    public static IEnumerable<IEnumerable<T>> CartesianProductV1<T>(
    this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IDeque<T>> result = [Deque<T>.Empty];
        foreach (var sequence in sequences)
            result = from deque in result
                     from item in sequence
                     select deque.PushRight(item);
        return result;
    }

    // Listing 6.6
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>
    (this IEnumerable<IEnumerable<T>> sequences) =>
    sequences.Aggregate(
        (IEnumerable<IDeque<T>>)[Deque<T>.Empty],
        (accumulator, sequence) =>
            from deque in accumulator
            from item in sequence
            select deque.PushRight(item));

}



