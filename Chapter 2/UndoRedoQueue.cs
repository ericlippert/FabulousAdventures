// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

using System.Collections;

static class UndoRedoQ
{
    public static void SampleCode()
    {
        Console.WriteLine("A mutable queue with undo-redo features built on top of an immutable queue");
        var u = new UndoRedoQueue<int>();
        Console.WriteLine(u.Bracket()); // []
        u.Enqueue(10);
        Console.WriteLine(u.Bracket()); // [10]
        u.Enqueue(20);
        Console.WriteLine(u.Bracket()); // [10, 20]
        u.Enqueue(30);
        Console.WriteLine(u.Bracket()); // [10, 20, 30]
        u.Undo();
        Console.WriteLine(u.Bracket()); // [10, 20]
        u.Redo();
        Console.WriteLine(u.Bracket()); // [10, 20, 30]
        u.Dequeue();
        Console.WriteLine(u.Bracket()); // [20, 30]
        u.Undo();
        Console.WriteLine(u.Bracket()); // [10, 20, 30]
    }
}

class UndoRedoQueue<T> : IEnumerable<T>
{
    private UndoRedo<IImQueue<T>> q = new UndoRedo<IImQueue<T>>(ImQueue<T>.Empty);
    public void Enqueue(T item) => q.Do(q.State.Enqueue(item));
    public T Peek() => q.State.Peek();
    public T Dequeue()
    {
        T item = q.State.Peek();
        q.Do(q.State.Dequeue());
        return item;
    }

    public bool IsEmpty => q.State.IsEmpty;
    public bool CanUndo => q.CanUndo;
    public void Undo() => q.Undo();
    public bool CanRedo => q.CanRedo;
    public void Redo() => q.Redo();
    public IEnumerator<T> GetEnumerator() => q.State.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
