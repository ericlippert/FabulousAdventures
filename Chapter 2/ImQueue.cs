
// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

using System.Collections;

static class ImmutableQueue
{
    public static void SampleCode()
    {
        Console.WriteLine("An immutable queue");
        var q1 = ImQueue<int>.Empty;
        var q2 = q1.Enqueue(10);
        var q3 = q2.Enqueue(20);
        var q4 = q3.Enqueue(30);
        var q5 = q4.Dequeue();
        var q6 = q5.Dequeue();
        var q7 = q6.Dequeue();

        Console.WriteLine(q1.Bracket());
        Console.WriteLine(q2.Bracket());
        Console.WriteLine(q3.Bracket());
        Console.WriteLine(q4.Bracket());
        Console.WriteLine(q5.Bracket());
        Console.WriteLine(q6.Bracket());
        Console.WriteLine(q7.Bracket());


        // []
        // [10]
        // [10, 20]
        // [10, 20, 30]
        // [20, 30]
        // [30]
        // []
    }
}

interface IImQueue<T> : IEnumerable<T>
{
    IImQueue<T> Enqueue(T item);
    T Peek();
    IImQueue<T> Dequeue();
    bool IsEmpty { get; }
}

// Listing 2.7: An immutable queue interface

class ImQueue<T> : IImQueue<T>
{
    public static IImQueue<T> Empty { get; } = new ImQueue<T>(ImStack<T>.Empty, ImStack<T>.Empty);

    private readonly IImStack<T> enqueues;
    private readonly IImStack<T> dequeues;
    private ImQueue(IImStack<T> enqueues, IImStack<T> dequeues)
    {
        this.enqueues = enqueues;
        this.dequeues = dequeues;
    }
    public IImQueue<T> Enqueue(T item) =>
        this.IsEmpty ?
        new ImQueue<T>(this.enqueues, this.dequeues.Push(item)) :
        new ImQueue<T>(this.enqueues.Push(item), this.dequeues);
    public T Peek() => this.dequeues.Peek();
    public IImQueue<T> Dequeue()
    {
        IImStack<T> newdeq = this.dequeues.Pop();
        if (!newdeq.IsEmpty)
            return new ImQueue<T>(this.enqueues, newdeq);
        if (this.enqueues.IsEmpty)
            return Empty;
        return new ImQueue<T>(ImStack<T>.Empty, this.enqueues.Reverse());
    }
    public bool IsEmpty => this.dequeues.IsEmpty;
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in this.dequeues)
            yield return item;
        foreach (var item in this.enqueues.Reverse())
            yield return item;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}







