// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 2

// A mutable undo-redo logic helper for immutable state.

class UndoRedo<T>
{
    private IImStack<T> undo = ImStack<T>.Empty;
    private IImStack<T> redo = ImStack<T>.Empty;
    public T State { get; private set; }
    public UndoRedo(T initial)
    {
        State = initial;
    }
    public void Do(T newState)
    {
        undo = undo.Push(State);
        State = newState;
        redo = ImStack<T>.Empty;
    }
    public bool CanUndo => !undo.IsEmpty;
    public T Undo()
    {
        if (!CanUndo)
            throw new InvalidOperationException();
        redo = redo.Push(State);
        State = undo.Peek();
        undo = undo.Pop();
        return State;
    }
    public bool CanRedo => !redo.IsEmpty;
    public T Redo()
    {
        if (!CanRedo)
            throw new InvalidOperationException();
        undo = undo.Push(State);
        State = redo.Peek();
        redo = redo.Pop();
        return State;
    }
}