// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

static class Chapter14
{
    public static void Samples()
    {
        Console.WriteLine("Chapter 14");
        Sample1();
        Sample2();
    }

    static void Sample1()
    {
        Console.WriteLine("\nSample 1\n---");
        List<Cold> cold = [Cold.No, Cold.Yes];
        var prior = cold.ToCategorical(90, 10);
        var marginal = prior.Bind(SneezedGivenCold);
        Console.WriteLine(marginal.ShowWeights());
    }

    static void Sample2()
    {
        Console.WriteLine("\nSample 2\n---");
        var d6 = DiscreteUniform.Distribution(6).Project(d => d + 1);
        var d10 = DiscreteUniform.Distribution(10).Project(d => d + 1);
        List<IDiscreteDistribution<int>> dice = [d6, d10];
        IDiscreteDistribution<IDiscreteDistribution<int>> deep =
          dice.ToCategorical(1, 1);
        IDiscreteDistribution<int> shallow = deep.Join();
        Console.WriteLine(shallow.ShowWeights());
    }
    static IDiscreteDistribution<Sneezed> SneezedGivenCold(Cold c)
    {
        List<Sneezed> sneezed = [Sneezed.No, Sneezed.Yes];
        return c == Cold.No ?
            sneezed.ToCategorical(97, 3) :
            sneezed.ToCategorical(14, 86);
    }

}

static partial class Extensions
{
    public static IDiscreteDistribution<R> Bind<T, R>(
        this IDiscreteDistribution<T> prior,
        Func<T, IDiscreteDistribution<R>> likelihood) where R : notnull =>
        prior.ToJoint(likelihood).Project(joint => joint.Item2);

    public static IDiscreteDistribution<T> Join<T>(
        this IDiscreteDistribution<IDiscreteDistribution<T>> deep) where T : notnull =>
        deep.Bind(x => x);

    public static IDiscreteDistribution<T> Where<T>(
        this IDiscreteDistribution<T> distribution,
        Func<T, bool> predicate) where T : notnull =>
        distribution.Bind(t => predicate(t) ?
            (IDiscreteDistribution<T>)Singleton<T>.Distribution(t) :
            Empty<T>.Distribution);
}
