// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness
public sealed class Categorical : IDiscreteDistribution<int>
{
    private readonly List<int> weights;
    private readonly IDiscreteDistribution<int> chooseRow;
    private readonly IDiscreteDistribution<int>[] distributions;
    public static IDiscreteDistribution<int> Distribution(
        params IEnumerable<int> weights) =>
        weights.All(x => x == 0) ? Empty<int>.Distribution : 
        new Categorical(weights.ToList());

    private Categorical(List<int> weights)
    {
        if (weights.Any(x => x < 0) || weights.All(x => x == 0))
            throw new InvalidOperationException();
        int gcd = weights.GCD();
        for (int i = 0; i < weights.Count; i += 1)
            weights[i] /= gcd;
        this.weights = weights;
        int total = weights.Sum();
        int n = weights.Count;
        chooseRow = DiscreteUniform.Distribution(n);
        distributions = new IDiscreteDistribution<int>[n];

        var tooNarrow = new Dictionary<int, int>();
        var tooWide = new Dictionary<int, int>();
        for (int row = 0; row < n; row += 1)
        {
            int width = weights[row] * n;
            if (width == total)
                distributions[row] = Singleton<int>.Distribution(row);
            else if (width < total)
                tooNarrow.Add(row, width);
            else
                tooWide.Add(row, width);
        }

        while (tooNarrow.Count > 0)
        {
            (int narrowRow, int narrowWidth) = tooNarrow.First();
            tooNarrow.Remove(narrowRow);

            (int wideRow, int wideWidth) = tooWide.First();
            tooWide.Remove(wideRow);

            int fromWide = total - narrowWidth;
            distributions[narrowRow] =
                Bernoulli.Distribution(narrowWidth, fromWide)
                    .Project(x => x == 0 ? narrowRow : wideRow);
            int newWidth = wideWidth - fromWide;

            if (newWidth == total) 
                distributions[wideRow] =
                    Singleton<int>.Distribution(wideRow);
            else if (newWidth < total)
                tooNarrow[wideRow] = newWidth;
            else
                tooWide[wideRow] = newWidth;
        }
    }

    public IEnumerable<int> Support() =>
        Enumerable.Range(0, weights.Count).Where(x => weights[x] != 0);

    public int Weight(int i) =>
        0 <= i && i < weights.Count ? weights[i] : 0;

    public int Sample() =>
        distributions[chooseRow.Sample()].Sample();
}
