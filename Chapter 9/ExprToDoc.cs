// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 9
// Pretty Printing
// Adapted from Strictly Pretty by Christian Lindig

using static Doc;

sealed class ExprToDoc : IExprVisitor<Doc>
{
    public ExprToDoc() { }
    private static readonly Doc comma = Text(",");
    private static readonly Doc lparen = Text("(");
    private static readonly Doc rparen = Text(")");
    private static readonly Doc star = Text("*");
    private static readonly Doc plus = Text("+");
    private static readonly Doc dot = Text(".");

    public Doc Visit(Id id) => Text(id.name);
    public Doc Visit(Paren paren) =>
        Group(Indent(lparen + OptBreak + paren.expr.Accept(this)) + OptBreak + rparen);

    private Doc BinOp(Expr left, Doc op, Expr right) =>
        Group(Indent(Group(left.Accept(this) + Space + op) + Space + right.Accept(this)));
    public Doc Visit(Add add) => BinOp(add.left, plus, add.right);
    public Doc Visit(Mult mult) => BinOp(mult.left, star, mult.right);
    
    public Doc Visit(Member member) =>
        Group(member.obj.Accept(this) + Indent(Group(OptBreak + dot + Text(member.name))));

    public Doc CommaList(IList<Expr> items)
    {
        Doc result = Empty;
        for (int i = 0; i < items.Count; i += 1)
        {
            result += items[i].Accept(this);
            if (i != items.Count - 1)
                result += comma + Space;
        }
        return result;
    }

    private Doc ParenList(IList<Expr> items) =>
        items.Count == 0 ?
            lparen + rparen :
            Group(Indent(lparen + OptBreak + CommaList(items) + rparen));

    public Doc Visit(Call call) =>
        call.receiver.Accept(this) + ParenList(call.args);
}

static class Extensions
{
    public static Doc ToDoc(this Expr expr) => 
        expr.Accept(new ExprToDoc());

    public static string Pretty(this Expr expr, int width) =>
        expr.ToDoc().Pretty(width);
}