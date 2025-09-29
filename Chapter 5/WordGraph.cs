// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

using System.Collections;
using System.Text;

sealed class WordGraph : IWords
{
    private Node root;
    public WordGraph(Node root)
    {
        this.root = root;
    }

    private Node? Find(string prefix)
    {
        Node? n = root;
        foreach (char c in prefix)
        {
            n = n.TryFollowEdge(c);
            if (n == null)
                break;
        }
        return n;
    }

    public IEnumerable<string> StartsWith(string prefix)
    {
        Node? n = Find(prefix);
        return n == null ? Enumerable.Empty<string>() : Words(n, new StringBuilder(prefix));
    }

    public bool Contains(string word) => Find(word)?.IsEnd ?? false;
    private IEnumerable<string> Words(Node n, StringBuilder prefix)
    {
        // postcondition: prefix will be reset to its current state
        if (n.IsEnd)
            yield return prefix.ToString();
        foreach (char c in n.EdgeLabels)
        {
            prefix.Append(c);
            foreach (var word in Words(n.FollowEdge(c), prefix))
                yield return word;
            prefix.Remove(prefix.Length - 1, 1);
        }
    }
    public IEnumerator<string> GetEnumerator() => StartsWith("").GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}