// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 9
// Pretty Printing
// Adapted from Strictly Pretty by Christian Lindig

interface IExprVisitor<T>
{
    T Visit(Id id);
    T Visit(Paren paren);
    T Visit(Add add);
    T Visit(Mult mult);
    T Visit(Member member);
    T Visit(Call call);
}

abstract record class Expr
{
    public abstract T Accept<T>(IExprVisitor<T> visitor);
}
sealed record class Id(string name) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

sealed record class Paren(Expr expr) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
sealed record class Add(Expr left, Expr right) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

sealed record class Mult(Expr left, Expr right) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

sealed record class Member(Expr obj, string name) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

sealed record class Call(Expr receiver, params IList<Expr> args) : Expr
{
    public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
