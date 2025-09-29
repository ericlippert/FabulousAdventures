// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 5

List<string> words = File.ReadAllLines(@"..\..\..\enable.txt").ToList();
// List<string> words = ["car", "care", "cares", "cars", "fir", "fire", "firer", "firers", "firs"];

var wl1 = new SortedWordList(words);
var wl2 = new WordGraph(TrieBuilder.MakeTrie(words));
var wl3 = new WordGraph(DawgBuilder.MakeDawg(words));
