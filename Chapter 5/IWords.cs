// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

interface IWords : IEnumerable<string>
{
    bool Contains(string word);
    IEnumerable<string> StartsWith(string prefix);
}