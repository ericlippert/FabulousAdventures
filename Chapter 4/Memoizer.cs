// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 4

sealed class Memoizer<A, R> where A : notnull
{
    private Dictionary<A, R> dict;
    private readonly Func<A, R> f;
    public Memoizer(Func<A, R> f)
    {
        this.dict = new Dictionary<A, R>();
        this.f = f;
    }
    public R MemoizedFunc(A a)
    {
        if (!dict.TryGetValue(a, out R? r))
        {
            r = f(a);
            dict.Add(a, r);
        }
        return r;
    }

    public void Clear(Dictionary<A, R>? newDict = null)
    {
        dict = newDict ?? new Dictionary<A, R>();
    }

    public int Count => dict.Count;
}