// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness


namespace RejectionCategorical
{
    public sealed class Categorical : IDiscreteDistribution<int>
    {
        private readonly List<int> weights;
        public static IDiscreteDistribution<int> Distribution(
            params IEnumerable<int> weights)
        {
            List<int> w = weights.ToList();
            if (w.Any(x => x < 0) || w.All(x => x == 0))
                throw new InvalidOperationException();
            int gcd = w.GCD();
            for (int i = 0; i < w.Count; i += 1)
                w[i] /= gcd;
            return new Categorical(w);
        }

        private Categorical(List<int> weights)
        {
            this.weights = weights;
        }

        public IEnumerable<int> Support() =>
            Enumerable.Range(0, weights.Count).Where(x => weights[x] != 0);

        public int Weight(int i) =>
          0 <= i && i < weights.Count ? weights[i] : 0;

        public int Sample()
        {
            var rows = DiscreteUniform.Distribution(weights.Count);
            var columns = DiscreteUniform.Distribution(weights.Max());
            while (true)
            {
                int row = rows.Sample();
                if (columns.Sample() < weights[row])
            return row;
            }
        }
    }
}