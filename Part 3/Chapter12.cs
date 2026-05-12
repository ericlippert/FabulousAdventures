// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
static class Chapter12
{

    sealed record class Dog(string Name);

    public static void Samples()
    {
        Console.WriteLine("Chapter 12");
        Sample1();
        Sample2();
        Sample3();
        Sample4();
        Sample5();
        Sample6();
        Sample7();
        Sample8();
        Sample9();
        Sample10();
        Sample11();
        Sample12();
    }


    static void Sample1()
    {
        Console.WriteLine("\nSample 1\n---");
        var DU6 = DiscreteUniform.Distribution(6);
        int roll = DU6.Sample();
        Console.WriteLine(roll);
    }

    static void Sample2()
    {
        Console.WriteLine("\nSample 2\n---");
        var coin = Bernoulli.Distribution(5, 7);
        int flip = coin.Sample();
        Console.WriteLine(flip);
    }

    static void Sample3()
    {
        Console.WriteLine("\nSample 3\n---");
        var DU6 = DiscreteUniform.Distribution(6);
        Console.WriteLine(DU6.Samples().Take(10).Comma());
        Console.WriteLine(DU6.Samples().Take(8).Sum());
    }

    static void Sample4()
    {
        Console.WriteLine("\nSample 4\n---");
        var coin = Bernoulli.Distribution(30, 70);
        Console.WriteLine(coin.Histogram());
        Console.WriteLine(coin.ShowWeights());
    }
    static void Sample5()
    {
        Console.WriteLine("\nSample 5\n---");
        string flip = Bernoulli.Distribution(5, 7).Sample() == 0 ?
            "heads" : "tails";
        Console.WriteLine(flip);
    }

    static void Sample6()
    {
        Console.WriteLine("\nSample 6\n---");
        List<string> colors = ["green", "blue", "red", "blue", "red", "blue"];
        var DU6 = DiscreteUniform.Distribution(6);
        var die = Projected<int, string>.Distribution(DU6, i => colors[i]);
        Console.WriteLine(die.ShowWeights());

    }

    static void Sample7()
    {
        Console.WriteLine("\nSample 7\n---");
        List<string> colors = ["green", "blue", "red", "blue", "red", "blue"];
        var die = DiscreteUniform.Distribution(6).Project(i => colors[i]);
        Console.WriteLine(die.ShowWeights());
        Console.WriteLine(die.Histogram());
    }

    static void Sample8()
    {
        Console.WriteLine("\nSample 8\n---");
        var cat = QuickAndDirtyCategorical.MakeCategorical(
            ["A", "B", "C", "D"],
            [101, 99, 51, 49]);
        Console.WriteLine(cat.Histogram());
    }

    static void Sample9()
    {
        Console.WriteLine("\nSample 9\n---");
        var cat2 = Ladder.Categorical.Distribution(101, 99, 51, 49);
        Console.WriteLine(cat2.Histogram());
        List<Dog> dogs = [new Dog("Rover"), new Dog("Fido"), new Dog("Lassie"), new Dog("Astro")];
        IDiscreteDistribution<Dog> randomDog = cat2.Project(i => dogs[i]);
        Console.WriteLine(randomDog.Sample().Name);
    }

    static void Sample10()
    {
        Console.WriteLine("\nSample 10\n---");
        Console.WriteLine(Categorical.Distribution(10, 0, 0, 11, 5).Histogram());
    }

    static void Sample11()
    {
        Console.WriteLine("\nSample 11\n---");
        List<string> names = ["Alice", "Bob", "Eve"];
        var randomName = names.ToCategorical(10, 30, 60);
        Console.WriteLine(randomName.Sample());
        Console.WriteLine(randomName.Histogram());
    }

    static void Sample12()
    {
        Console.WriteLine("\nSample 12\n---");
        List<string> colors = ["green", "blue", "red"];
        var die = colors.ToCategorical(1, 3, 2);
        Console.WriteLine(die.Histogram());
        Console.WriteLine();
        Console.WriteLine(die.Filter(roll => roll != "green").Histogram());
    }
}

static partial class Extensions
{
    public static int GCD(this int x, int y)
    {
        while (y != 0)
            (x, y) = (y, x % y);
        return x;
    }
    public static int GCD(this IEnumerable<int> numbers) =>
        numbers.Aggregate(0, GCD);

    public static IEnumerable<T> Samples<T>(
        this IDistribution<T> distribution)
    {
        while (true)
            yield return distribution.Sample();
    }

    public static string Comma<T>(this IEnumerable<T> ts) =>
        string.Join(",", ts);


    public static string Newlines<T>(this IEnumerable<T> ts) =>
        string.Join("\n", ts);

    public static string PadLeft<T>(this T t, int width) where T : notnull =>
        t.ToString()!.PadLeft(width);

    public static string Histogram<T>(
        this IDiscreteDistribution<T> d) where T : notnull
        => d.Samples().Take(100000).Histogram();

    public static string Histogram<T>(this IEnumerable<T> d) where T : notnull
    {
        const int barMax = 40;
        var dict = d.GroupBy(x => x)
                    .ToDictionary(g => g.Key, g => g.Count());
        int width = dict.Keys
            .Max(x => x.ToString()!.Length);
        int valueMax = dict.Values.Max();
        double scale = valueMax < barMax ?
            1.0 : ((double)barMax) / valueMax;
        string Bar(T t) => new('*', (int)(dict[t] * scale));
        return dict.Keys
            .OrderBy(x => x)
            .Select(s => $"{s.PadLeft(width)}|{dict[s],-5}|{Bar(s)}")
            .Newlines();
    }

    public static string ShowWeights<T>(
        this IDiscreteDistribution<T> d) where T : notnull
    {
        int labelWidth = d.Support()
            .Max(x => x.ToString()!.Length);
        return d.Support()
            .Select(s => $"{s.PadLeft(labelWidth)}:{d.Weight(s)}")
            .Newlines();
    }

    public static IDiscreteDistribution<R> Project<T, R>(
        this IDiscreteDistribution<T> underlying,
        Func<T, R> projection) where R : notnull =>
    Projected<T, R>.Distribution(underlying, projection);

    public static IDiscreteDistribution<T> ToCategorical<T>(
        this IList<T> categories, params IEnumerable<int> weights) where T : notnull =>
        Categorical.Distribution(weights).Project(i => categories[i]);

    public static IDiscreteDistribution<T> Filter<T>(
        this IDiscreteDistribution<T> distribution,
        Func<T, bool> predicate) where T : notnull
    {
        var newSupport = distribution.Support().Where(predicate).ToList();
        var newWeights = newSupport.Select(t => distribution.Weight(t));
        return newSupport.ToCategorical(newWeights);
    }
}