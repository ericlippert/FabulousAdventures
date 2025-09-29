// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5
sealed class TrieBuilder
{
    private Node root = new();
    private TrieBuilder() { }
    static public Node MakeTrie(IEnumerable<string> words)
    {
        var tb = new TrieBuilder();
        foreach (var word in words)
            tb.Add(word);
        return tb.root;
    }
    private void Add(string word)
    {
        Node n = root;
        foreach (char c in word)
            n = n.TryAddNewNode(c);
        n.IsEnd = true;
    }
}
