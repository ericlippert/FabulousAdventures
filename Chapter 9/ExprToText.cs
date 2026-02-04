// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 9
// Pretty Printing
// Adapted from Strictly Pretty by Christian Lindig

static class ExprToText
{
    public static void SampleCode()
    {
        var f = new Member(new Member(new Id("Frobozz"), "Frobulator"), "Frobulate");
        var b = new Member(new Id("Blob"), "Width");
        var c = new Member(new Id("Cob"), "Width");
        var g = new Member(new Id("Glob"), "Length");
        var q = new Member(new Member(new Id("Qbert"), "Quux"), "Qix");
        var m = new Mult(new Paren(new Add(b, c)), g);
        var call = new Call(f, m, q);
        Console.WriteLine("         1         2         3         4         5         6");
        Console.WriteLine("123456789012345678901234567890123456789012345678901234567890");
        Console.WriteLine(call.Pretty(80));
        Console.WriteLine("         1         2         3         4         5         6");
        Console.WriteLine("123456789012345678901234567890123456789012345678901234567890"); 
        Console.WriteLine(call.Pretty(40));
        Console.WriteLine("         1         2         3         4         5         6");
        Console.WriteLine("123456789012345678901234567890123456789012345678901234567890");
        Console.WriteLine(call.Pretty(29));
    }
}
