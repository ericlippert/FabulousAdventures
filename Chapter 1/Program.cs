// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 1

// An immutable linked list

var list = LinkedList<int>.Empty.Push(10).Push(20).Push(30).Push(40).Push(50);
var list2 = list.Reverse();
// Reversing the list does not change the list.
Console.WriteLine(list);
// 50 40 30 20 10
// 10 20 30 40 50
Console.WriteLine(list2);
