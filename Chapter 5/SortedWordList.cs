// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

using System.Collections;
class SortedWordList : IWords
{
    private List<string> words;
    public SortedWordList(List<string> words)
    {
        this.words = words;
    }

    public bool Contains(string word) => words.BinarySearch(word) >= 0;

    public IEnumerable<string> StartsWith(string prefix)
    {
        int i = words.BinarySearch(prefix);
        if (i < 0)
            i = ~i;
        for (; i < words.Count; i += 1)
        {
            string w = words[i];
            if (!w.StartsWith(prefix))
                yield break;
            yield return w;
        }
    }

    public IEnumerator<string> GetEnumerator() => words.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}