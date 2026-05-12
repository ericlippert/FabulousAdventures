// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
public sealed class ContinuousUniform : IContinuousDistribution
{
    private readonly double min;
    private readonly double max;
    private readonly Random rnd = new();
    public static readonly ContinuousUniform Standard = new(0.0, 1.0);
    public static ContinuousUniform Distribution(double min, double max) =>
        new(min, max);

    private ContinuousUniform(double min, double max)
    {
        if (min >= max)
            throw new ArgumentException();
        this.min = min;
        this.max = max;
    }

    public double Weight(double x) =>
        min <= x && x <= max ? 1.0 : 0.0;

    public double Sample() => rnd.NextDouble() * (max - min) + min;
}
