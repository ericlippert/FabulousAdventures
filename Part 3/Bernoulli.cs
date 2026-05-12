// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public sealed class Bernoulli : IDiscreteDistribution<int>
{
    private readonly DiscreteUniform du;
    public int Zero { get; }
    public int One { get; }

    public static IDiscreteDistribution<int> Distribution(
      int zero, int one)
    {
        if (zero < 0 || one < 0 || zero == 0 && one == 0)
            throw new InvalidOperationException();
        if (zero == 0)
            return Singleton<int>.Distribution(1);
        if (one == 0)
            return Singleton<int>.Distribution(0);
        int gcd = zero.GCD(one);
        return new Bernoulli(zero / gcd, one / gcd);
    }
    private Bernoulli(int zero, int one)
    {
        this.Zero = zero;
        this.One = one;
        this.du = DiscreteUniform.Distribution(zero + one);
    }
    public int Sample() => this.du.Sample() < Zero ? 0 : 1;
    public IEnumerable<int> Support() => [0, 1];
    public int Weight(int x) => x == 0 ? Zero : x == 1 ? One : 0;
}
