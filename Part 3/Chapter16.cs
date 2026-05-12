// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
using System.Numerics;
using static System.Math;

static class Chapter16
{
    public static void Samples()
    {
        Console.WriteLine("Chapter 16");
        Sample1();
        Sample2();
        Sample3();
        Sample4();
    }

    static void Sample1()
    {
        Console.WriteLine("\nSample 1\n---");
        var initial = DiscreteUniform.Distribution(4).Project(x => x + 4);
        var gamblersRuin = Markov<int>.Distribution(initial, GamblersTransition);
        Console.WriteLine(gamblersRuin.Sample().Comma());
        Console.WriteLine();
        Console.WriteLine(gamblersRuin.Samples().Take(100000)
            .Select(x => x.Count()).Histogram());

    }

    static void Sample2()
    {
        Console.WriteLine("\nSample 2\n---");
        Console.WriteLine("Real Shakespeare:\n");
        var sentences = File.ReadLines("shakespeare.txt").Sentences();
        foreach (var sentence in sentences.Skip(106).Take(6))
            Console.WriteLine(sentence.Space());

        Console.WriteLine("\nRandom Shakespeare:\n");
        var mb = new MarkovBuilder<string>();
        foreach (var sentence in sentences)
        {
            mb.AddInitial(sentence[0]);
            for (int i = 0; i < sentence.Count - 1; i += 1)
                mb.AddTransition(sentence[i], sentence[i + 1]);
        }
        var markov = mb.ToDistribution();
        Console.WriteLine(markov.Samples().Take(8)
            .Select(x => x.Space()).Newlines());
    }

    static void Sample3()
    {
        Console.WriteLine("\nSample 3\n---");
        var target = (double x) =>
    Exp(-x * x) + Exp((1.0 - x) * (x - 1.0) * 10.0);
        var initial = Normal.Standard;
        var proposal = (double current) => Normal.Distribution(current, 1.0);
        var mixture = Metropolis.Distribution(target, initial, proposal);
        Console.WriteLine(mixture.Histogram(-2.0, 3.0));
    }

    static void Sample4()
    {
        Console.WriteLine("\nSample 4\n---");
        var prior = Normal.Distribution(0.5, 0.2).Clamp(0.0, 1.0);
        var likelihood = (double fairness) => Binomial.Distribution(10, fairness);
        var posterior = prior.Posterior(likelihood);

        Console.WriteLine(prior.Histogram(0.0, 1.0));
        Console.WriteLine();
        Console.WriteLine(posterior(9).Histogram(0.0, 1.0));
    }

    static IDistribution<int> GamblersTransition(int pocket) =>
        (pocket == 0 || pocket == 10) ?
            Empty<int>.Distribution :
            DiscreteUniform.Distribution(2)
                .Project(x => x == 0 ? pocket - 1 : pocket + 1);

}

static partial class Extensions
{
    static readonly char[] punct = "*()[]#:;_\"".ToCharArray();
    public static IEnumerable<List<string>> Sentences(
        this IEnumerable<string> lines)
    {
        List<string> current = [];
        foreach (var line in lines)
        {
            foreach (var word in line.Split())
            {
                var trimmed = word.Trim(punct);
                if (trimmed.Length > 0)
                {
                    current.Add(trimmed);
                    char c = trimmed[trimmed.Length - 1];
                    if (c == '.' || c == '!' || c == '?')
                    {
                        yield return current;
                        current = [];
                    }
                }
            }
        }
    }
    public static string Space<T>(this IEnumerable<T> ts) =>
        string.Join(" ", ts);

    public static Func<B, IContinuousDistribution> Posterior<B>(
        this IContinuousDistribution prior,
        Func<double, IWeightedDistribution<B>> likelihood,
        double stddev = 1.0)
    {
        return (B evidence) =>
        {
            Func<double, double> posteriorPDF = a =>
                prior.Weight(a) * likelihood(a).Weight(evidence);
            Func<double, IContinuousDistribution> proposal =
                current => Normal.Distribution(current, stddev);
            return Metropolis.Distribution(posteriorPDF, prior, proposal);
        };
    }

    public static BigInteger Choose(this int n, int k)
    {
        if (k > n)
            return 0;
        if (k > n / 2)
            k = n - k;
        BigInteger result = 1;
        for (int i = 1; i <= k; i += 1)
            result = result * (n - k + i) / i;
        return result;
    }

}

