// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 9
// Pretty Printing
// Adapted from Strictly Pretty by Christian Lindig

using static Doc;
static class DocToText
{
    public static void SampleCode()
    {
        // Foo.Bar(xx + yy * zz). 

        TextDoc foo = new("Foo"), bar = new("Bar"), xx = new("xx"), 
            yy = new("yy"), zz = new("zz"), dot = new("."), plus = new("+"), 
            star = new("*"), lparen = new("("), rparen = new(")");
        var mult = Group(Indent(Group(yy + Space + star) + Space + zz));
        var sum = Group(Indent(Group(xx + Space + plus) + Space + mult));
        var parens = Group(Indent(lparen + OptBreak + sum + rparen));
        var foobar = Group(foo + Indent(Group(OptBreak + dot + bar)));
        var callfb = foobar + parens;

        Console.WriteLine(callfb.Pretty(30));
        Console.WriteLine(callfb.Pretty(20));
        Console.WriteLine(callfb.Pretty(15));
    }
}

