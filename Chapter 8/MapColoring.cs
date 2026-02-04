// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8

using System.Collections.Immutable;
using System.Drawing;

static class MapColoring
{
    public static void SampleCode()
    {
        // Listing 8.3
        Console.WriteLine("Listing 8.3");

        var southAmerica = ImGraph<string>.Empty
            .AddNode("Falkland Islands")
            .AddEdges("French Guiana", ["Brazil", "Suriname"])
            .AddEdges("Suriname", ["Brazil", "Guyana"])
            .AddEdges("Guyana", ["Brazil", "Venezuela"])
            .AddEdges("Venezuela", ["Brazil", "Colombia"])
            .AddEdges("Colombia", ["Brazil", "Peru", "Ecuador"])
            .AddEdges("Peru", ["Brazil", "Ecuador", "Bolivia", "Chile"])
            .AddEdges("Chile", ["Bolivia", "Argentina"])
            .AddEdges("Bolivia", ["Brazil", "Paraguay", "Argentina"])
            .AddEdges("Paraguay", ["Brazil", "Argentina"])
            .AddEdges("Argentina", ["Brazil", "Uruguay"])
            .AddEdge("Uruguay", "Brazil");

        var colors = ImmutableHashSet<Color>.Empty
            .Add(Color.Red).Add(Color.Yellow)
            .Add(Color.Green).Add(Color.Purple);

        var simple = southAmerica.ColorGraph(colors);
        foreach (var n in simple!.Keys)
            Console.WriteLine($"{n} -> {simple[n].Name}");

        // Sample code in 8.5.4
        Console.WriteLine("Section 8.5.4 sample code");

        var islands = ImGraph<string>.Empty.AddNode("A").AddNode("B");
        var islandsReducer = new GraphColorReducer<string, Color>(islands);
        var islandsBT = new Backtracker<string, Color>(islandsReducer);
        var islandsInit = ImMulti<string, Color>.Empty
            .SetItem("A", colors).SetItem("B", colors);
        foreach (var s in islandsBT.Solve(islandsInit))
            Console.Write($"({s["A"].Single().Name},{s["B"].Single().Name}) ");
        Console.WriteLine();
    }
}
