// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

// An immutable stack with Push, Peek, Pop, IsEmpty operations.

using System.Collections;
static class ImmutableStack
{
    public static void SampleCode()
    {
        Console.WriteLine("An immutable stack");
        var s1 = ImStack<int>.Empty;
        var s2 = s1.Push(10);
        var s3 = s2.Push(20);
        var s4 = s2.Push(30); // Push s2 again; s2 doesn't change
        var s5 = s4.Pop();
        var s6 = s5.Pop();

        Console.WriteLine(s1.Bracket()); // []
        Console.WriteLine(s2.Bracket()); // [10]
        Console.WriteLine(s3.Bracket()); // [20, 10]
        Console.WriteLine(s4.Bracket()); // [30, 10]
        Console.WriteLine(s5.Bracket()); // [10]
        Console.WriteLine(s6.Bracket()); // []
        Console.WriteLine();
    }
}

// Sometimes we'll define abstract data types with interfaces first;
// other times we'll just go straight to a class.
interface IImStack<T> : IEnumerable<T>
{
    IImStack<T> Push(T item);
    T Peek();
    IImStack<T> Pop();
    bool IsEmpty { get; }
}

class ImStack<T> : IImStack<T>
{
    private class EmptyStack : IImStack<T>
    {
        public EmptyStack() { }
        public IImStack<T> Push(T item) => new ImStack<T>(item, this);
        public T Peek() => throw new InvalidOperationException();
        public IImStack<T> Pop() => throw new InvalidOperationException();
        public bool IsEmpty => true;
        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public static IImStack<T> Empty { get; } = new EmptyStack();

    private readonly T item;
    private readonly IImStack<T> tail;
    private ImStack(T item, IImStack<T> tail)
    {
        this.item = item;
        this.tail = tail;
    }
    public IImStack<T> Push(T item) => new ImStack<T>(item, this);
    public T Peek() => item;
    public IImStack<T> Pop() => tail;
    public bool IsEmpty => false;
    public IEnumerator<T> GetEnumerator()
    {
        for (IImStack<T> s = this; !s.IsEmpty; s = s.Pop())
            yield return s.Peek();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}