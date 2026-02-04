// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

// Listing 8.6
interface IReducer<K, V> where K : notnull where V : notnull
{
    ImMulti<K, V> Reduce(ImMulti<K, V> solver);
}
