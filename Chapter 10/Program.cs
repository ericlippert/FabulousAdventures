// Fabulous Adventures In Data Structures and Algorithms
// Chapter 10
// Unification

// Comparing Unification Algorithms in First-Order Theorem Proving
// Krystof Hoder and Andrei Voronkov

Sample1();
Sample2();
Sample3();

void Sample1()
{
    Symbol ImStack(BinTerm left, BinTerm right) =>
        new("ImStack", left, right);

    Hole X = new("X"), Y = new("Y");
    Constant two = new(2), emptyStack = new("EmptyStack");
    var stackOne = ImStack(X, emptyStack);
    var term1 = ImStack(X, stackOne);
    var term2 = ImStack(two, Y);
    Console.WriteLine(term1);
    Console.WriteLine(term2);
    var subst = term1.Unify(term2);
    foreach (var hole in subst.Keys)
        Console.WriteLine($"{hole.Name} -> {subst[hole]}");
    Console.WriteLine(term1.Substitute(subst));
    Console.WriteLine(term2.Substitute(subst));
}
void Sample2()
{
    Symbol Blah(BinTerm left, BinTerm right) =>
        new("Blah", left, right);

    Hole W = new ("W"), X = new("X"), Y = new("Y"), Z = new("Z");
    var two = new Constant(2);
    var term1 = Blah(W, Blah(X, Blah(Y, Blah(Z, two))));
    var term2 = Blah(Blah(Blah(Blah(two, Z), Y), X), W);
    Console.WriteLine(term1);
    Console.WriteLine(term2);
    var subst = term1.Unify(term2);
    foreach (var hole in subst.Keys)
        Console.WriteLine($"{hole.Name} -> {subst[hole]}");
    Console.WriteLine(term1.Substitute(subst));
    Console.WriteLine(term2.Substitute(subst));
}
void Sample3()
{
    var list = new Constant("list");
    var dble = new Constant("double");
    var func = new Constant("Func");
    Symbol Frob(BinTerm ret, BinTerm sig) =>
        new Symbol("Frob", ret, sig);
    Symbol Generic(BinTerm name, BinTerm args) => 
        new Symbol("Generic", name, args);
    Symbol Signature(BinTerm generics, BinTerm parameters) => 
        new Symbol("Signature", generics, parameters);
    Symbol Comma(BinTerm c1, BinTerm c2) => 
        new Symbol("Comma", c1, c2);
    Hole A = new("A"), B = new("B"), T = new("T"), U = new("U"), 
        V = new("V"), X = new("X");
    var term1 = Frob(Generic(list, B), Signature(Comma(A, B), Comma(A, Generic(func, Comma(A, B)))));
    var term2 = Frob(V, Signature(Comma(T, U), Comma(dble, Generic(func, Comma(X, Generic(list, X))))));
    var subst = term1.Unify(term2);
    Console.WriteLine($"T -> {T.Substitute(subst)}");
    Console.WriteLine($"U -> {U.Substitute(subst)}");
    Console.WriteLine($"V -> {V.Substitute(subst)}");
    Console.WriteLine($"X -> {X.Substitute(subst)}");
}