// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

using static System.Math;
public sealed class Normal : IContinuousDistribution
{
    private readonly double mean;
    private readonly double stddev;
    public readonly static Normal Standard = Distribution(0, 1);
    public static Normal Distribution(double mean, double stddev) =>
        new(mean, stddev);
    private Normal(double mean, double stddev)
    {
        this.mean = mean;
        this.stddev = stddev;
    }
    private static double StandardSample()
    {
        var radius = Sqrt(-2.0 * Log(ContinuousUniform.Standard.Sample()));
        var angle = ContinuousUniform.Distribution(0, 2 * PI).Sample();
        return radius * Cos(angle);
    }
    private static double StandardWeight(double x) => Exp(-x * x / 2.0);
    public double Sample() => stddev * StandardSample() + mean;
    public double Weight(double x) => StandardWeight((x - mean) / stddev);
}
