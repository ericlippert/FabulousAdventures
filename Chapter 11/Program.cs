// Fabulous Adventures In Data Structures and Algorithms
// Chapter 11
// Anti-Unification

Symbol Mult(BinTerm t1, BinTerm t2) => new("Mult", t1, t2);
Symbol Add(BinTerm t1, BinTerm t2) => new("Add", t1, t2);
Symbol Div(BinTerm t1, BinTerm t2) => new("Div", t1, t2);
Constant one = new(1), two = new(2), five = new(5), six = new(6);
BinTerm term1 = Mult(Add(one, two), Div(Add(one, two), five));
BinTerm term2 = Mult(six, Div(six, five));
var au = term1.AntiUnify(term2);
Console.WriteLine(au.Result);
Console.WriteLine(au.Result.Substitute(au.Sub1));
Console.WriteLine(au.Result.Substitute(au.Sub2));
