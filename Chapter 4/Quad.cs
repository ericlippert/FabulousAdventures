// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 4

using System.Diagnostics;
using QuadPoint = (long X, long Y);

// An n-quad is an immutable square grid 2-to-the-n on a side, 
// so 4-to-the-n cells total.
//
// There are two 0-quads, which we'll call "Alive" and "Dead".
//
// An n-quad for n > 0 consists of four (n-1)-quads labeled NW, NE, SE, SW, for
// the northwest, northeast, southeast and southwest quadrants of the grid.
//
// All of this code is a private implementation detail rather than public surface
// area that must be robust against misuse, so invariant violations will result
// in assertions, not exceptions; any assertion is a bug.
//
// There is in principle no reason why we cannot make arbitrarily large quads,
// but in order to keep all the math in the range of a long, I'm going to
// restrict quads to level 60 or less. 

// Is this an onerous restriction? I know it seems like 4 to the 60
// addressible cells ought to be enough for anyone, but there are good reasons
// why you want to allow larger quads that we will go into later in this series.
// Were we doing a more powerful implementation of this algorithm then we'd
// do the math in big integers instead and allow arbitrarily large grids.

interface IQuad
{
    IQuad NW { get; }
    IQuad NE { get; }
    IQuad SE { get; }
    IQuad SW { get; }
    int Level { get; }
    bool IsEmpty { get; }
}

sealed class Quad : IQuad
{
    private sealed class Leaf : IQuad
    {
        public IQuad NW => throw new InvalidOperationException();
        public IQuad NE => throw new InvalidOperationException();
        public IQuad SE => throw new InvalidOperationException();
        public IQuad SW => throw new InvalidOperationException();
        public int Level => 0;
        public bool IsEmpty => this == Dead;
    }
    public static IQuad Dead { get; } = new Leaf();
    public static IQuad Alive { get; } = new Leaf();

    public const int MaxLevel = 60;
    public IQuad NW { get; }
    public IQuad NE { get; }
    public IQuad SE { get; }
    public IQuad SW { get; }

    // We could compute level recursively of course, but this is frequently 
    // accessed so we'll cache it in the object and use an extra field.
    public int Level { get; }
    public bool IsEmpty => this == Empty(Level);
    private Quad(IQuad nw, IQuad ne, IQuad se, IQuad sw)
    {
        NW = nw;
        NE = ne;
        SE = se;
        SW = sw;
        Level = nw.Level + 1;
    }

    public static Memoizer<(IQuad, IQuad, IQuad, IQuad), IQuad> 
        MakeQuadMemoizer { get; } =
        new Memoizer<(IQuad, IQuad, IQuad, IQuad), IQuad>(UnmemoizedMake);

    private static IQuad UnmemoizedMake(
        (IQuad nw, IQuad ne, IQuad se, IQuad sw) args)
    {
        if (args.nw.Level != args.ne.Level ||
            args.ne.Level != args.se.Level ||
            args.se.Level != args.sw.Level ||
            args.nw.Level >= MaxLevel)
            throw new InvalidOperationException();
        return new Quad(args.nw, args.ne, args.se, args.sw);
    }

    public static IQuad Make(IQuad nw, IQuad ne, IQuad se, IQuad sw) =>
        MakeQuadMemoizer.MemoizedFunc((nw, ne, se, sw));
    public static Memoizer<int, IQuad> EmptyMemoizer { get; } = 
        new Memoizer<int, IQuad>(UnmemoizedEmpty);

    private static IQuad UnmemoizedEmpty(int level)
    {
        if (level == 0)
            return Dead;
        var e = Empty(level - 1);
        return Make(e, e, e, e);
    }

    public static IQuad Empty(int level) =>
        EmptyMemoizer.MemoizedFunc(level);
}

static class Extensions
{
    public static long Width(this IQuad quad) =>
        1L << quad.Level;

    // Since we have memoized all quads, it is impossible to know given just a quad
    // what its coordinates are; multiple quads cannot both be referentially 
    // identical and also know their distinct locations. 

    // Let's start by establishing a convention. Given an n-quad that represents
    // the "center" of the infinite plane, let's say that the lower left corner
    // is (-Width/2, -Width/2).  For instance, suppose we have a 2-quad that is
    // "centered" on the origin. We assign coordinates to it as follows:
    //
    // (-2,  1) | (-1,  1) || (0,  1) | (1,  1)
    // --------------------||------------------
    // (-2,  0) | (-1,  0) || (0,  0) | (1,  0)
    // ========================================
    // (-2, -1) | (-1, -1) || (0, -1) | (1, -1)
    // --------------------||------------------
    // (-2, -2) | (-1, -2) || (0, -2) | (1, -2)
    //
    // Note that the double lines demarcate 1-quads and the single lines 
    // demarcate 0-quads.

    public static bool Contains(this IQuad quad, QuadPoint p)
    {
        if (quad.Level == 0)
            return p == (0, 0);
        long w = quad.Width() / 2;
        return -w <= p.X && p.X < w && -w <= p.Y && p.Y < w;
    }

    // Note that I'm creating "Get" and "Set" methods here rather than adding a
    // user-defined indexer because the semantics of indexing are mutation, and I want
    // to emphasize here that we are immutable.

    public static IQuad Get(this IQuad quad, QuadPoint p)
    {
        if (!quad.Contains(p))
            throw new InvalidOperationException();
        if (quad.Level == 0)
            return quad;
        if (quad.IsEmpty)
            return Quad.Dead;
        long w = quad.Width() / 4;
        QuadPoint newp = (
            quad.Level == 1 ? 0 : 0 <= p.X ? p.X - w : p.X + w,
            quad.Level == 1 ? 0 : 0 <= p.Y ? p.Y - w : p.Y + w);
        if (0 <= p.X)
            if (0 <= p.Y)
                return quad.NE.Get(newp);
            else
                return quad.SE.Get(newp);
        else if (0 <= p.Y)
            return quad.NW.Get(newp);
        else
            return quad.SW.Get(newp);
    }


    // The setter does not mutate anything, since this is an immutable data structure; rather,
    // it returns either the same object if there was no change, or a new object with the change.
    // Since the vast majority of the references in the new object are the same as the old,
    // we typically have only rebuilt the "spine".

    public static IQuad Set(this IQuad quad, QuadPoint p, IQuad q)
    {
        Debug.Assert(q != null && q.Level == 0);

        if (!quad.Contains(p))
            throw new InvalidOperationException();

        if (quad.Level == 0)
            return q;
        if (q == Quad.Dead && quad.IsEmpty)
            return quad;
        long w = quad.Width() / 4;
        QuadPoint newp = (
            quad.Level == 1 ? 0 : 0 <= p.X ? p.X - w : p.X + w,
            quad.Level == 1 ? 0 : 0 <= p.Y ? p.Y - w : p.Y + w);
        var nw = quad.NW;
        var ne = quad.NE;
        var se = quad.SE;
        var sw = quad.SW;
        if (0 <= p.X)
            if (0 <= p.Y)
                ne = quad.NE.Set(newp, q);
            else
                se = quad.SE.Set(newp, q);
        else if (0 <= p.Y)
            nw = quad.NW.Set(newp, q);
        else
            sw = quad.SW.Set(newp, q);
        return Quad.Make(nw, ne, se, sw);
    }


    // We have the NW, NE, SE and SW quads in hand, but we will also need to
    // be able to get the N, S, E, W and center quads.
    public static IQuad Center(this IQuad quad) =>
        Quad.Make(quad.NW.SE, quad.NE.SW, quad.SE.NW, quad.SW.NE);
    public static IQuad North(this IQuad quad) =>
        Quad.Make(quad.NW.NE, quad.NE.NW, quad.NE.SW, quad.NW.SE);
    public static IQuad East(this IQuad quad) =>
        Quad.Make(quad.NE.SW, quad.NE.SE, quad.SE.NE, quad.SE.NW);
    public static IQuad South(this IQuad quad) =>
        Quad.Make(quad.SW.NE, quad.SE.NW, quad.SE.SW, quad.SW.SE);
    public static IQuad West(this IQuad quad) =>
        Quad.Make(quad.NW.SW, quad.NW.SE, quad.SW.NE, quad.SW.NW);

    public static bool HasAllEmptyEdges(this IQuad quad) =>
        quad.NW.NW.IsEmpty && quad.NW.NE.IsEmpty &&
        quad.NE.NW.IsEmpty && quad.NE.NE.IsEmpty &&
        quad.NE.SE.IsEmpty && quad.SE.NE.IsEmpty &&
        quad.SE.SE.IsEmpty && quad.SE.SW.IsEmpty &&
        quad.SW.SE.IsEmpty && quad.SW.SW.IsEmpty &&
        quad.SW.NW.IsEmpty && quad.NW.SW.IsEmpty;   

    // Given an n-quad, give me back an (n+1) quad with the original
    // quad in the center, and a ring of empty cells surrounding it.

    public static IQuad Embiggen(this IQuad quad)
    {
        if (quad.Level == 0 || quad.Level >= Quad.MaxLevel)
            throw new InvalidOperationException();
        var e = Quad.Empty(quad.Level - 1);
        return Quad.Make(
            Quad.Make(e, e, quad.NW, e),
            Quad.Make(e, e, e, quad.NE),
            Quad.Make(quad.SE, e, e, e),
            Quad.Make(e, quad.SW, e, e));
    }



    // ---------

    //
    // Drawing to bitmaps
    //

    // Assuming that this quad is a grid centered on the origin, 
    // call setPixel for every living cell inside the given
    // rectangle.
    public static void Draw(this IQuad quad, LifeRect r, Action<QuadPoint> setPixel, int scale)
    {
        long w = -quad.Width() / 2;
        quad.Draw((w,w), r, setPixel, scale);
    }

    // Assuming that this quad is a grid whose bottom left corner
    // has the given coordinates, call setPixel for every living 
    // cell inside the given rectangle.
    private static void Draw(this IQuad quad, QuadPoint lowerLeft, LifeRect rect, Action<QuadPoint> setPixel, int scale)
    {
        Debug.Assert(scale >= 0);
        // Easy out; if we're an empty grid then there are no pixels to draw.
        if (quad.IsEmpty)
            return;

        // Does this quad overlap the rectangle at all? If not, then we have
        // no work to do.

        // Remember that the rectangle has the coordinates of the top left
        // corner of the rectangle, but we are given the bottom left corner,
        // so we have a small amount of math to do.
        long w = quad.Width();
        if (!rect.Overlaps(new LifeRect((lowerLeft.X, lowerLeft.Y + w - 1), w, w)))
            return;

        if (quad.Level <= scale)
        {
            setPixel(lowerLeft);
            return;
        }

        quad.NW.Draw((lowerLeft.X, lowerLeft.Y + w/2), rect, setPixel, scale);
        quad.NE.Draw((lowerLeft.X + w/2, lowerLeft.Y + w/2), rect, setPixel, scale);
        quad.SE.Draw((lowerLeft.X + w/2, lowerLeft.Y), rect, setPixel, scale);
        quad.SW.Draw(lowerLeft, rect, setPixel, scale);
    }
}

