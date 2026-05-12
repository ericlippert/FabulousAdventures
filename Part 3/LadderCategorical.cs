// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Part 3: A better abstraction for randomness

using System.Diagnostics;

namespace Ladder
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
            int height = DiscreteUniform.Distribution(weights.Sum()).Sample();
            int nextRungHeight = 0;
            int rung = 0;
            foreach (int weight in weights)
            {
                nextRungHeight += weight;
                if (height < nextRungHeight)
                    return rung;
                rung += 1;
            }
            throw new UnreachableException();
        }
    }
}