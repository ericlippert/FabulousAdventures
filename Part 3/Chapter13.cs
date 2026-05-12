// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

enum Cold { No, Yes }
enum Sneezed { No, Yes }

enum Frob { No, Yes }
enum CheapTest { Negative, Positive }

enum Coin { Normal, DoubleHeaded }
enum Flip { Tails, Heads }


static class Chapter13
{

    public static void Samples()
    {
        Console.WriteLine("Chapter 13");
        Sample1();
        Sample2();
        Sample3();
        Sample4();
        Sample5();
        Sample6();
    }


    static void Sample1()
    {
        Console.WriteLine("\nSample 1\n---");
        List<Cold> cold = [Cold.No, Cold.Yes];
        var prior = cold.ToCategorical(90, 10);
        var cs = prior.Samples().Take(10000)
            .Select(c => (c, SneezedGivenCold(c).Sample()));
        Console.WriteLine(cs.Histogram());
        Console.WriteLine();
        prior = cold.ToCategorical(80, 20);
        cs = prior.Samples().Take(10000)
            .Select(c => (c, SneezedGivenCold(c).Sample()));
        Console.WriteLine(cs.Histogram());
    }

    static void Sample2()
    {
        Console.WriteLine("\nSample 2\n---");
        List<Cold> cold = [Cold.No, Cold.Yes];
        var prior = cold.ToCategorical(90, 10);
        Console.WriteLine(prior.ToJoint(SneezedGivenCold).ShowWeights());
        Console.WriteLine();
        prior = cold.ToCategorical(70, 30);
        Console.WriteLine(prior.ToJoint(SneezedGivenCold).ShowWeights());

    }

    static void Sample3()
    {
        Console.WriteLine("\nSample 3\n---");
        List<Cold> cold = [Cold.No, Cold.Yes];
        var prior = cold.ToCategorical(90, 10);
        var posterior = prior.Posterior(SneezedGivenCold);
        Console.WriteLine(posterior(Sneezed.Yes).ShowWeights());
    }

    static void Sample4()
    {
        Console.WriteLine("\nSample 4\n---");
        List<Frob> frob = [Frob.No, Frob.Yes];
        var frobPrior = frob.ToCategorical(29999, 1);
        var frobPosterior = frobPrior.Posterior(TestResultGivenFrob);
        Console.WriteLine(frobPosterior(CheapTest.Positive).ShowWeights());
        Console.WriteLine();
        Console.WriteLine(frobPrior.ToJoint(TestResultGivenFrob).ShowWeights());
    }

    static void Sample5()
    {
        Console.WriteLine("\nSample 5\n---");
        List<Coin> coins = [Coin.Normal, Coin.DoubleHeaded];
        var tenflips = coins.ToCategorical(999, 1)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads)
            .Posterior(FlipGivenCoin)(Flip.Heads);
        Console.WriteLine(tenflips.ShowWeights());
    }

    static void Sample6()
    {
        Console.WriteLine("\nSample 6\n---");
        var d20 = DiscreteUniform.Distribution(20).Project(d => d + 1);
        var advantage = d20.ToJoint(_ => d20)
        .Project(pair => Math.Max(pair.Item1, pair.Item2));
        Console.WriteLine(advantage.ShowWeights());
        Console.WriteLine();
        advantage = d20.JointProject(_ => d20, Math.Max);
        Console.WriteLine(advantage.ShowWeights());
    }


    public static IDiscreteDistribution<Sneezed> SneezedGivenCold(Cold c)
    {
        List<Sneezed> sneezed = [Sneezed.No, Sneezed.Yes];
        return c == Cold.No ?
            sneezed.ToCategorical(97, 3) :
            sneezed.ToCategorical(14, 86);
    }

    public static IDiscreteDistribution<CheapTest> TestResultGivenFrob(Frob f)
    {
        List<CheapTest> cheap = [CheapTest.Negative, CheapTest.Positive];
        return f == Frob.No ?
            cheap.ToCategorical(995, 5) :
            cheap.ToCategorical(5, 995);
    }

    public static IDiscreteDistribution<Flip> FlipGivenCoin(Coin c)
    {
        List<Flip> flip = [Flip.Tails, Flip.Heads];
        return c == Coin.Normal ?
            flip.ToCategorical(1, 1) :
            Singleton<Flip>.Distribution(Flip.Heads);
    }
}

static partial class Extensions
{
    public static int LCM(this int x, int y) =>
        x * y / x.GCD(y);

    public static int LCM(this IEnumerable<int> numbers) =>
        numbers.Aggregate(1, LCM);

    public static int TotalWeight<T>(
        this IDiscreteDistribution<T> distribution) =>
        distribution.Support().Sum(s => distribution.Weight(s));

    public static IDiscreteDistribution<(A, B)> ToJoint<A, B>(
        this IDiscreteDistribution<A> prior,
        Func<A, IDiscreteDistribution<B>> likelihood)
    {
        int lcm = prior.Support()
            .Select(a => likelihood(a).TotalWeight())
            .Where(w => w != 0)
            .LCM();
        List<(A, B)> jointSupport = [];
        List<int> jointWeight = [];
        foreach (A a in prior.Support())
        {
            var db = likelihood(a);
            foreach (B b in db.Support())
            {
                jointSupport.Add((a, b));
                jointWeight.Add(
                    prior.Weight(a) * db.Weight(b) * lcm /
                    db.TotalWeight());
            }
        }
        return jointSupport.ToCategorical(jointWeight);
    }

    public static Func<B, IDiscreteDistribution<A>> Posterior<A, B>(
        this IDiscreteDistribution<A> prior,
        Func<A, IDiscreteDistribution<B>> likelihood) where A : notnull =>
        b => prior.ToJoint(likelihood)
                .Filter(joint =>
                     EqualityComparer<B>.Default.Equals(joint.Item2, b))
                .Project(joint => joint.Item1);

    public static IDiscreteDistribution<C> JointProject<A, B, C>(
        this IDiscreteDistribution<A> prior,
        Func<A, IDiscreteDistribution<B>> likelihood,
        Func<A, B, C> projection) where C : notnull =>
        prior.ToJoint(likelihood)
             .Project(pair => projection(pair.Item1, pair.Item2));


}