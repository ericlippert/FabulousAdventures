// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 8


using System.Collections.Immutable;

static class SudokuSolver
{
    public static void SampleCode()
    {

        // Listing 8.5

        var sudoku = ImGraph<int>.Empty;
        // Rows
        for (int i = 0; i < 9; i += 1)
            sudoku = sudoku.AddClique([.. Enumerable.Range(i * 9, 9)]);
        // Columns
        for (int i = 0; i < 9; i += 1)
            sudoku = sudoku.AddClique([.. Enumerable.Range(0, 9).Select(x => x * 9 + i)]);
        // Boxes
        sudoku = sudoku
            .AddClique([00, 01, 02, 09, 10, 11, 18, 19, 20])
            .AddClique([03, 04, 05, 12, 13, 14, 21, 22, 23])
            .AddClique([06, 07, 08, 15, 16, 17, 24, 25, 26])
            .AddClique([27, 28, 29, 36, 37, 38, 45, 46, 47])
            .AddClique([30, 31, 32, 39, 40, 41, 48, 49, 50])
            .AddClique([33, 34, 35, 42, 43, 44, 51, 52, 53])
            .AddClique([54, 55, 56, 63, 64, 65, 72, 73, 74])
            .AddClique([57, 58, 59, 66, 67, 68, 75, 76, 77])
            .AddClique([60, 61, 62, 69, 70, 71, 78, 79, 80]);

        var digits = ImmutableHashSet<char>.Empty
            .Add('1').Add('2').Add('3').Add('4').Add('5')
            .Add('6').Add('7').Add('8').Add('9');


        // Listing 8.8
        Console.WriteLine("Listing 8.8");

        string puzzle =
            "  8 274  " +
            "         " +
            " 6 38  52" +
            "      32 " +
            "1   7   4" +
            " 92      " +
            "78  62 1 " +
            "         " +
            "  384 5  ";

        var initial = ImMulti<int, char>.Empty;
        foreach (var cell in sudoku.Nodes)
            initial = initial.SetItem(cell, digits);
        for (int i = 0; i < puzzle.Length; i += 1)
            if (puzzle[i] != ' ')
                initial = initial.SetSingle(i, puzzle[i]);

        var reducer = new GraphColorReducer<int, char>(sudoku);
        var backtracker = new Backtracker<int, char>(reducer);
        var solution = backtracker.Solve(initial).First();
        for (int i = 0; i < 81; i += 1)
        {
            Console.Write(solution[i].Single());
            if (i % 9 == 8)
                Console.WriteLine();
        }
    }
}