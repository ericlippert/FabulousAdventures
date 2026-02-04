// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

using System.Collections.Immutable;

// Listing 8.1 
readonly struct ImMulti<K, V> where K : notnull where V : notnull
{
    private readonly ImmutableDictionary<K, ImmutableHashSet<V>> dict;

    public static ImMulti<K, V> Empty =
        new(ImmutableDictionary<K, ImmutableHashSet<V>>.Empty);

    private ImMulti(ImmutableDictionary<K, ImmutableHashSet<V>> dict) =>
        this.dict = dict;

    public bool IsEmpty => dict.IsEmpty;

    public bool ContainsKey(K k) => dict.ContainsKey(k);

    public bool HasValue(K k, V v) =>
        dict.ContainsKey(k) && dict[k].Contains(v);

    public IEnumerable<K> Keys => dict.Keys;

    public ImmutableHashSet<V> this[K k] => dict[k];

    public ImMulti<K, V> SetItem(K k, ImmutableHashSet<V> vs) =>
        new(dict.SetItem(k, vs));

    public ImMulti<K, V> SetSingle(K k, V v) =>
        SetItem(k, ImmutableHashSet<V>.Empty.Add(v));

    public ImMulti<K, V> SetEmpty(K k) =>
        SetItem(k, ImmutableHashSet<V>.Empty);

    public ImMulti<K, V> Add(K k, V v) =>
        ContainsKey(k) ?
            new(dict.SetItem(k, dict[k].Add(v))) :
            SetSingle(k, v);

    public ImMulti<K, V> Remove(K k) => new(dict.Remove(k));

    public ImMulti<K, V> Remove(K k, V v) =>
        new(dict.SetItem(k, dict[k].Remove(v)));
}
