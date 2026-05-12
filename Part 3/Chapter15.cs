// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

using System.Text;
using static System.Math;
static class Chapter15
{
    public static void Samples()
    {
        Console.WriteLine("Chapter 15");
        Sample1();
        Sample2();
        Sample3();
        Sample4();
        Sample5();
    }

    static void Sample1()
    {
        Console.WriteLine("\nSample 1\n---");
        Console.WriteLine(ContinuousUniform.Standard.Histogram(0.0, 1.0));
    }


    static void Sample2()
    {
        Console.WriteLine("\nSample 2\n---");
        Console.WriteLine(Normal.Distribution(60.0, 0.1).Histogram(59.6, 60.4));
    }

    static void Sample3()
    {
        Console.WriteLine("\nSample 3\n---");
        Console.WriteLine(Cauchy.Standard.Histogram(-4.0, 4.0));
    }

    static void Sample4()
    {
        Console.WriteLine("\nSample 4\n---");
        var target = (double x) =>
            Exp(-x * x) + Exp((1.0 - x) * (x - 1.0) * 10.0);
        var mixture = new Rejection(target, Normal.Standard, 2.5);
        Console.WriteLine(mixture.Histogram(-2.0, 3.0));

    }

    static void Sample5()
    {
        Console.WriteLine("\nSample 5\n---");
        var original = Normal.Distribution(0.5, 0.2);
        var clamped = original.Clamp(0.0, 1.0);
        Console.WriteLine(original.Histogram(-0.5, 1.5));
        Console.WriteLine(clamped.Histogram(-0.5, 1.5));
    }
}

static partial class Extensions
{
    public static string Histogram(
        this IContinuousDistribution d, double low, double high) =>
        d.Samples().Take(100000).Histogram(low, high);

    public static string Histogram(
        this IEnumerable<double> d, double low, double high)
    {
        const int width = 40;
        const int height = 20;
        int[] columns = new int[width];
        foreach (double sample in d)
        {
            int column = (int)(width * (sample - low) / (high - low));
            if (0 <= column && column < width)
                columns[column] += 1;
        }
        int max = columns.Max();
        double scale = max < height ? 1.0 : ((double)height) / max;
        var sb = new StringBuilder();
        for (int row = height - 1; row >= 0; row -= 1)
        {
            foreach (var b in columns)
                sb.Append(b * scale > row ? '*' : ' ');
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static IContinuousDistribution Clamp(
        this IContinuousDistribution original, double min, double max) =>
        new Rejection(
            (double d) => min <= d && d <= max ? original.Weight(d) : 0.0,
            original,
            1.0);

}