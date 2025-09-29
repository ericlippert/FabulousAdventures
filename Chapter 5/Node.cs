// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

sealed class Node
{
    private readonly Dictionary<char, Node> edges = [];
    public bool IsEnd { get; set; }
    public IEnumerable<char> EdgeLabels => edges.Keys;
    public Node FollowEdge(char c) => edges[c];
    public Node? TryFollowEdge(char c) => HasEdge(c) ? FollowEdge(c) : null;
    public bool HasEdge(char c) => edges.ContainsKey(c);
    public Node AddNewNode(char c) => edges[c] = new();
    public Node TryAddNewNode(char c) => HasEdge(c) ? FollowEdge(c) : AddNewNode(c);
    public int Count => edges.Count;
    public char LastEdgeLabel => EdgeLabels.Max();
    public Node ReplaceEdge(char c, Node n) => edges[c] = n;
}
