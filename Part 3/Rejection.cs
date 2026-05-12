// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

public sealed record class Rejection(
    Func<double, double> Target,
    IContinuousDistribution Helper,
    double Scale
    ) : IContinuousDistribution
{
    public double Weight(double x) => Target(x);
    public double Sample()
    {
        while (true)
        {
            double x = Helper.Sample();
            double weight = Weight(x);
            if (weight == 0.0)
                continue;
            double max = Helper.Weight(x) * Scale;
            double y = ContinuousUniform.Distribution(0.0, max).Sample();
            if (y <= weight)
                return x;
        }
    }
}
