// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 4

using static Quad;
using static System.Linq.Enumerable;
using static System.Math;
using System.Diagnostics;
using QuadPoint = (long X, long Y);

// Implementation of Gosper's HashLife algorithm
sealed class HashLife : ILife, IReport, IDrawScale
{
    private IQuad cells = Empty(9);
    private int stepCallCount = 0;

    public HashLife() { }

    public void Clear() => cells = Empty(9);

    public bool this[long x, long y]
    {
        get => cells.Contains((x, y)) && cells.Get((x, y)) != Dead;
        set
        {
            while (!cells.Contains((x, y)))
                cells = cells.Embiggen();
            cells = cells.Set((x, y), value ? Alive : Dead);
        }
    }

    // One more time, the life rule. Given a level-zero quad
    // and the count of its living neighbours, it stays the
    // same if the count is two, it stays or becomes alive if the 
    // count is three, and it stays or becomes dead otherwise:
    // 
    private static IQuad Rule(IQuad q, int count) => 
        count switch { 2 => q, 3 => Alive, _ => Dead };

    // The basic idea of Gosper's algorithm is that if we have an n-quad,
    // then we have enough information to compute the next state of the 
    // (n-1) quad that is its *center*.
    //
    // We can do this recursively but of course we need a base case. 
    // A 0-quad and a 1-quad do not have enough cells, so our
    // smallest possible base case is a 2-quad.

    private static IQuad StepBaseCase(IQuad q)
    {
        // We have a 2-quad, which is a 4x4 grid. 
        //
        // a b c d
        // e f g h
        // i j k l
        // m n o p

        // We wish to compute the 1-quad that is the next state of the
        // four middle cells f g j k

        // First get the state of all 16 cells as an integer:

        Debug.Assert(q.Level == 2);
        int a = (q.NW.NW == Dead) ? 0 : 1;
        int b = (q.NW.NE == Dead) ? 0 : 1;
        int c = (q.NE.NW == Dead) ? 0 : 1;
        int d = (q.NE.NE == Dead) ? 0 : 1;
        int e = (q.NW.SW == Dead) ? 0 : 1;
        int f = (q.NW.SE == Dead) ? 0 : 1;
        int g = (q.NE.SW == Dead) ? 0 : 1;
        int h = (q.NE.SE == Dead) ? 0 : 1;
        int i = (q.SW.NW == Dead) ? 0 : 1;
        int j = (q.SW.NE == Dead) ? 0 : 1;
        int k = (q.SE.NW == Dead) ? 0 : 1;
        int l = (q.SE.NE == Dead) ? 0 : 1;
        int m = (q.SW.SW == Dead) ? 0 : 1;
        int n = (q.SW.SE == Dead) ? 0 : 1;
        int o = (q.SE.SW == Dead) ? 0 : 1;
        int p = (q.SE.SE == Dead) ? 0 : 1;

        // The neighbours of cell f are cells a, b, c, ...
        // Add them up.

        int nf = a + b + c + e + g + i + j + k;
        int ng = b + c + d + f + h + j + k + l;
        int nj = f + g + h + j + l + n + o + p;
        int nk = e + f + g + i + k + m + n + o;

        return Make(
            Rule(q.NW.SE, nf), Rule(q.NE.SW, ng), Rule(q.SE.NW, nj), Rule(q.SW.NE, nk));
    }

    private static IQuad UnmemoizedStep((IQuad q, int speed) args)
    {
        // This algorithm moves forward 2-to-the-speed ticks on the 
        // center of quad q.
        //
        // There are two possibilities; either we are running at maximum
        // speed, which is speed equal to level - 2, or we are running
        // slower than that.
        //
        // If we are running at maximum speed then when we recurse,
        // we need to reduce speed.
        //
        // If we are running at slower than maximum speed, we can do
        // the recursion at current speed, to get the center bits at the correct
        // number of ticks forwards, and then extract the bits from there.

        IQuad q = args.q;
        int speed = args.speed;

        Debug.Assert(q.Level >= 2);
        Debug.Assert(speed >= 0);
        Debug.Assert(speed <= q.Level - 2);

        IQuad r;
        if (q.IsEmpty)
            r = Quad.Empty(q.Level - 1);
        else if (speed == 0 && q.Level == 2)
            r = StepBaseCase(q);
        else
        {
            // Do we need to slow down on the recursion?

            int nineSpeed = (speed == q.Level - 2) ? speed - 1 : speed;

            Debug.Assert(q.NW != null && q.NE != null && q.SW != null && q.SE != null);

            IQuad q9nw = Step(q.NW, nineSpeed);
            IQuad q9n = Step(q.North(), nineSpeed);
            IQuad q9ne = Step(q.NE, nineSpeed);
            IQuad q9w = Step(q.West(), nineSpeed);
            IQuad q9c = Step(q.Center(), nineSpeed);
            IQuad q9e = Step(q.East(), nineSpeed);
            IQuad q9sw = Step(q.SW, nineSpeed);
            IQuad q9s = Step(q.South(), nineSpeed);
            IQuad q9se = Step(q.SE, nineSpeed);
            IQuad q4nw = Make(q9nw, q9n, q9c, q9w);
            IQuad q4ne = Make(q9n, q9ne, q9e, q9c);
            IQuad q4se = Make(q9c, q9e, q9se, q9s);
            IQuad q4sw = Make(q9w, q9c, q9s, q9sw);

            // Do we already have the result we need, or should
            // we run forwards as fast as possible?

            if (speed == q.Level - 2)
            {
                IQuad rnw = Step(q4nw, speed - 1);
                IQuad rne = Step(q4ne, speed - 1);
                IQuad rse = Step(q4se, speed - 1);
                IQuad rsw = Step(q4sw, speed - 1);
                r = Make(rnw, rne, rse, rsw);
            }
            else
            {
                IQuad rnw = q4nw.Center();
                IQuad rne = q4ne.Center();
                IQuad rse = q4se.Center();
                IQuad rsw = q4sw.Center();
                r = Make(rnw, rne, rse, rsw);
            }
        }
        Debug.Assert(q.Level == r.Level + 1);
        return r;
    }

    private int maxCache = 200000;
    static Memoizer<(IQuad, int), IQuad> StepSpeedMemoizer { get; } = new Memoizer<(IQuad, int), IQuad>(UnmemoizedStep);

    private static IQuad Step(IQuad q, int speed) =>
        StepSpeedMemoizer.MemoizedFunc((q, speed));


    // Step forward 2 to the n ticks.
    public void Step(int speed = 0)
    {
        const int MaxSpeed = MaxLevel - 2;
        if (speed < 0 || speed > MaxSpeed)
            throw new InvalidOperationException();

        bool resetMaxCache = false;
        stepCallCount += 1;
        if ((stepCallCount & 0x1ff) == 0)
        {
            int cacheSize = Quad.MakeQuadMemoizer.Count + StepSpeedMemoizer.Count;
            if (cacheSize > maxCache)
            {
                resetMaxCache = true;
                ResetCaches();
            }
        }
        IQuad current = cells;
        if (!current.HasAllEmptyEdges())
            current = current.Embiggen().Embiggen();
        else if (!current.Center().HasAllEmptyEdges())
            current = current.Embiggen();
        while (current.Level < speed + 2)
            current = current.Embiggen();

        IQuad next = Step(current, speed);
        // Remember, this is now one level smaller than current.
        // We might as well bump it up; we're just going to check
        // its edges for emptiness on the next tick again.
        cells = next.Embiggen();

        if (resetMaxCache)
        {
            int cacheSize = Quad.MakeQuadMemoizer.Count + StepSpeedMemoizer.Count;
            maxCache = Max(maxCache, cacheSize * 2);
        }
    }


    private static void ResetCaches()
    {
        StepSpeedMemoizer.Clear();
        var d = new Dictionary<(IQuad, IQuad, IQuad, IQuad), IQuad>();
        for (int i = 0; i < MaxLevel; i += 1)
        {
            var q = Empty(i);
            d[(q, q, q, q)] = Empty(i + 1);
        }
        Quad.MakeQuadMemoizer.Clear(d);
    }

    // --------

    public int MaxScale => 50;

    public void Draw(LifeRect rect, Action<QuadPoint> setPixel)
    {
        Draw(rect, setPixel, 0);
    }

    public void Draw(LifeRect rect, Action<QuadPoint> setPixel, int scale)
    {
        cells.Draw(rect, setPixel, scale);
    }

    public string Report() =>
        $"steps {stepCallCount}\n" +
        $"ssm {StepSpeedMemoizer.Count}\n" +
        $"mqm {Quad.MakeQuadMemoizer.Count}\n" +
        $"max {maxCache}\n";
}
