
// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

sealed class DawgBuilder
{
    private class NodeEquality : IEqualityComparer<Node>
    {
        public bool Equals(Node? x, Node? y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            // For our purposes two nodes are equal if and only if their 
            // their IsEnd properties are equal, and all outgoing edges are reference equal.
            if (x.IsEnd != y.IsEnd || x.Count != y.Count)
                return false;
            return x.EdgeLabels.All(e => y.HasEdge(e) && ReferenceEquals(x.FollowEdge(e), y.FollowEdge(e)));
        }

        public int GetHashCode(Node n)
        {
            var h = new HashCode();
            h.Add(n.IsEnd);
            foreach (var e in n.EdgeLabels.OrderBy(c => c))
            {
                h.Add(e);
                h.Add(n.FollowEdge(e));
            }
            return h.ToHashCode();
        }
    }

    private Node root = new();
    private HashSet<Node> canonicalNodes = new HashSet<Node>(new NodeEquality());
    private DawgBuilder() { }

    public static Node MakeDawg(IEnumerable<string> words)
    {
        var db = new DawgBuilder();
        // Precondition: words is sorted
        foreach (var word in words)
            db.Add(word);
        // Fix up the previously added word.
        db.FixUpLastWord(db.root);
        return db.root;
    }

    private void Add(string word)
    {
        int i = 0;
        Node n = root;

        // Find the common prefix (possibly empty)
        for (; i < word.Length; i += 1)
        {
            if (!n.HasEdge(word[i]))
                break;
            n = n.FollowEdge(word[i]);
        }

        // Fix the suffix of the previously added word.
        // The prefix will get fixed later.
        FixUpLastWord(n);

        // Finish adding the new word. 
        for (; i < word.Length; i += 1)
            n = n.AddNewNode(word[i]);
        n.IsEnd = true;

        // The word we just added is now the new last word in the graph. 
        // Neither the prefix nor the suffix have been fixed up yet.
        // We'll do it next time.

    }

    private void FixUpLastWord(Node n)
    {
        if (n.Count == 0)
            return;

        char c = n.LastEdgeLabel;
        Node s = n.FollowEdge(c);
        // Ensure that all nodes below n are fixed up.
        FixUpLastWord(s);

        Node? canonical;
        canonicalNodes.TryGetValue(s, out canonical);

        // If there is no node in the canonical set that matches s, then we have
        // a new canonical node; add s to the set.
        // Otherwise, there is a node in a previous word that exactly matches
        // s, so we can make n refer to that node instead of s.
        if (canonical == null)
            // Talk about rules of hash sets.
            canonicalNodes.Add(s);
        else
            n.ReplaceEdge(c, canonical);
    }
}
