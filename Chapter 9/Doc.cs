// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 9
// Pretty Printing
// Adapted from Strictly Pretty by Christian Lindig

using System.Text;

sealed record class TextDoc(string text) : Doc;
sealed record class OptBreakDoc(string text) : Doc;
sealed record class GroupDoc(Doc doc) : Doc;
sealed record class IndentDoc(Doc doc) : Doc;
sealed record class ConcatDoc(Doc left, Doc right) : Doc;
abstract record class Doc
{
    public static readonly int IndentSpaces = 4;
    public static readonly Doc Empty = new TextDoc("");
    public static readonly Doc Space = new OptBreakDoc(" ");
    public static readonly Doc OptBreak = new OptBreakDoc("");
    public static TextDoc Text(string text) => new TextDoc(text);
    public static GroupDoc Group(Doc doc) => new GroupDoc(doc);
    public static IndentDoc Indent(Doc doc) => new IndentDoc(doc);
    public static ConcatDoc Concat(Doc left, Doc right) => new ConcatDoc(left, right);
    public static ConcatDoc operator +(Doc left, Doc right) => Concat(left, right);

    // If NO optional breaks were line breaks, would this doc fit in the given width, or not?
    private bool Fits(int width)
    {
        var docs = new Stack<Doc>();
        docs.Push(this);
        int used = 0;  // How many characters have we already used on the current line?
        while (true)
        {
            if (used > width)
                return false;
            if (docs.Count == 0)
                return true;
            Doc doc = docs.Pop();
            switch (doc)
            {
                case TextDoc td:
                    used += td.text.Length;
                    break;
                case GroupDoc gd:
                    docs.Push(gd.doc);
                    break;
                case IndentDoc id:
                    docs.Push(id.doc);
                    break;
                case OptBreakDoc bd:
                    used += bd.text.Length;
                    break;
                case ConcatDoc cd:
                    docs.Push(cd.right);
                    docs.Push(cd.left);
                    break;
            }
        }
    }
    public string Pretty(int width)
    {
        var sb = new StringBuilder();
        int used = 0; // How many characters have we already used on the current line?
        var docs = new Stack<(bool, int, Doc)>();
        docs.Push((true, 0, new GroupDoc(this)));
        while (docs.Count > 0)
        {
            (bool fits, int indent, Doc doc) = docs.Pop();
            switch (doc)
            {
                case TextDoc td:
                    sb.Append(td.text);
                    used += td.text.Length;
                    break;
                case GroupDoc gd:
                    docs.Push((gd.doc.Fits(width - used), indent, gd.doc));
                    break;
                case IndentDoc id:
                    docs.Push((fits, indent + IndentSpaces, id.doc));
                    break;
                case OptBreakDoc bd:
                    if (fits)
                    {
                        sb.Append(bd.text);
                        used += bd.text.Length;
                    }
                    else
                    {
                        sb.AppendLine();
                        sb.Append(' ', indent);
                        used = indent;
                    }
                    break;
                case ConcatDoc cd:
                    docs.Push((fits, indent, cd.right));
                    docs.Push((fits, indent, cd.left));
                    break;
            }
        }
        return sb.ToString();
    }
}