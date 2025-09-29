// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 4

using static System.Math;
using QuadPoint = (long X, long Y);

class BoolArrayLife : ILife, IDraw
{
    // Allocate two bigger than we want to keep a ring of zero around the outside
    private const int height = 258;
    private const int width = 258;
    private byte[,] cells = new byte[width, height];        
    private byte[,] temp = new byte[width, height];
    public BoolArrayLife() {}
    public void Clear()
    {
        cells = new byte[width, height];
    }
    public bool this[long x, long y]
    {
        get
        {
            if (0 < x && x < cells.GetLength(0) - 1 && 
                0 < y && y < cells.GetLength(1) - 1)
                return cells[x, y] != 0;
            return false;
        }
        set
        {
            if (0 < x && x < cells.GetLength(0) - 1 && 
                0 < y && y < cells.GetLength(1) - 1)
                cells[x, y] = value ? (byte)1 : (byte)0;
        }
    }

    public void Step(int speed = 0)
    {
        long steps = 1L << speed;
        for (long i = 0; i < steps; i += 1)
        {
            for (int y = 1; y < cells.GetLength(1) - 1; y += 1)
            {
                for (int x = 1; x < cells.GetLength(0) - 1; x += 1)
                {
                    int count = cells[x - 1, y - 1] + cells[x - 1, y]
                        + cells[x - 1, y + 1] + cells[x, y - 1]
                        + cells[x, y + 1] + cells[x + 1, y - 1]
                        + cells[x + 1, y] + cells[x + 1, y + 1];
                    temp[x, y] = count == 3 || 
                        (cells[x, y] != 0 && count == 2) ? 
                        (byte)1 : (byte)0;
                }
            }
            var t = temp;
            temp = cells;
            cells = t;
        }
    }

    public void Draw(LifeRect rect, Action<QuadPoint> setPixel)
    {
        long xmin = Max(0, rect.X);
        long xmax = Min(cells.GetLength(0), rect.X + rect.Width);
        long ymin = Max(0, rect.Y - rect.Height + 1);
        long ymax = Min(cells.GetLength(1), rect.Y + 1);
        for (long y = ymin; y < ymax; y += 1)
            for (long x = xmin; x < xmax; x += 1)
                if (cells[x, y] != 0)
                    setPixel((x, y));
    }
}
