// Fabulous Adventures In Data Structures and Algorithms
// Chapter 11
// Anti-Unification

using Substitution = System.Collections.Generic.Dictionary<Hole, BinTerm>;

abstract record class BinTerm
{
    public bool IsBoundHole(Substitution subst) =>
        this is Hole h && subst.ContainsKey(h);

    public BinTerm Substitute(Substitution subst)
    {
        if (this.IsBoundHole(subst))
            return subst[(Hole)this].Substitute(subst);
        if (this is Symbol s)
        {
            var left = s.Left.Substitute(subst);
            var right = s.Right.Substitute(subst);
            if (!ReferenceEquals(left, s.Left) || !ReferenceEquals(right, s.Right))
                return new Symbol(s.Name, left, right);
        }
        return this;
    }

    public IEnumerable<Hole> AllHoles()
    {
        var stack = new Stack<BinTerm>();
        stack.Push(this);
        var seen = new HashSet<Hole>();
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current is Hole h && !seen.Contains(h))
            {
                seen.Add(h);
                yield return h;
            }
            else if (current is Symbol s)
            {
                stack.Push(s.Right);
                stack.Push(s.Left);
            }
        }
    }
    public BinTerm LookUpBoundHole(Substitution subst)
    {
        // If a term is a hole that we have a substitution for, look it up.
        // If that is *also* a bound hole, keep going until we either have
        // an unbound hole, symbol, or constant.
        var newTerm = this;
        while (newTerm.IsBoundHole(subst))
            newTerm = subst[(Hole)newTerm];
        return newTerm;
    }

    public Substitution? Unify(BinTerm t2)
    {
        var subst = new Substitution();
        return Unify(this, t2, subst) ? subst : null;
    }

    private static bool Unify(BinTerm t1, BinTerm t2, Substitution subst)
    {
        t1 = t1.LookUpBoundHole(subst);
        t2 = t2.LookUpBoundHole(subst);
        if (t1 == t2)
            return true;
        if (t1 is Hole h1 && !h1.OccursIn(t2, subst))
        {
            subst.Add(h1, t2);
            return true;
        }
        if (t2 is Hole h2 && !h2.OccursIn(t1, subst))
        {
            subst.Add(h2, t1);
            return true;
        }
        if (t1 is Symbol s1 && t2 is Symbol s2 && s1.Name == s2.Name)
            return Unify(s1.Left, s2.Left, subst) && Unify(s1.Right, s2.Right, subst);
        return false;
    }

    public AntiUnification AntiUnify(BinTerm term2) => 
        AntiUnification.AntiUnify(this, term2);
}

sealed record class Hole(string Name) : BinTerm
{
    public bool OccursIn(BinTerm term, Substitution subst)
    {
        // true if this occurs in term under subst, false otherwise
        foreach (Hole hole in term.AllHoles())
        {
            if (this == hole)
                return true;
            if (hole.IsBoundHole(subst) && this.OccursIn(subst[hole], subst))
                return true;
        }
        return false;
    }
    public override string ToString() => Name;
}

sealed record class Constant(object Value) : BinTerm
{
    public override string ToString() => $"{Value}";
}
sealed record class Symbol(string Name, BinTerm Left, BinTerm Right) : BinTerm
{
    public override string ToString() => $"{Name}({Left},{Right})";
}
