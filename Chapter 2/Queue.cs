// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

// A mutable queue wrapping an immutable queue

using System.Collections;
class Queue<T> : IEnumerable<T>
{
    private IImQueue<T> q = ImQueue<T>.Empty;
    public void Enqueue(T item)
    {
        q = q.Enqueue(item);
    }
    public T Peek() => this.q.Peek();
    public T Dequeue()
    {
        T item = Peek();
        q = q.Dequeue();
        return item;
    }

    public bool IsEmpty => q.IsEmpty;

    public IEnumerator<T> GetEnumerator() => this.q.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}