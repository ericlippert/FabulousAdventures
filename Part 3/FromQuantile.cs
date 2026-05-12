// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

using static System.Math;
public class FromQuantile : IContinuousDistribution
{
    Func<double, double> pdf;
    Func<double, double> quantile;
    public FromQuantile(
        Func<double, double> pdf, Func<double, double> quantile)
    {
        this.pdf = pdf;
        this.quantile = quantile;
    }
    public double Sample() =>
        quantile(ContinuousUniform.Standard.Sample());
    public double Weight(double x) => pdf(x);
}

public sealed class Cauchy : FromQuantile
{
    static double PDF(double x) => 1.0 / (1.0 + x * x);
    static double Quantile(double p) => Tan(PI * (p - 0.5));
    public readonly static Cauchy Standard = new();
    private Cauchy() : base(PDF, Quantile) { }
}

