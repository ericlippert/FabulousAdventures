// Fabulous Adventures In Data Structures and Algorithms
// Chapter 11
// Anti-Unification

using Substitution = System.Collections.Generic.Dictionary<Hole, BinTerm>;

sealed class AntiUnification
{
    private readonly HoleMaker hm;
    public BinTerm Result { get; private set; }
    public Substitution Sub1 { get; init; }
    public Substitution Sub2 { get; init; }

    private AntiUnification(IEnumerable<Hole> holes)
    {
        this.hm = new(holes);
        this.Sub1 = [];
        this.Sub2 = [];
    }
    public static AntiUnification AntiUnify(BinTerm term1, BinTerm term2)
    {
        AntiUnification au = new(term1.AllHoles().Concat(term2.AllHoles()));
        au.Result = au.AU(term1, term2);
        return au;
    }

    private BinTerm AU(BinTerm term1, BinTerm term2)
    {
        // Case one: equal terms are already anti-unified:
        if (term1 == term2)
            return term1;

        // Case two: unequal terms that are symbols with the same name anti-unify recursively:
        if(term1 is Symbol s1 && term2 is Symbol s2 && s1.Name == s2.Name)
            return new Symbol(s1.Name, AU(s1.Left, s2.Left), AU(s1.Right, s2.Right));

        // Case three: if we already have a hole for this, use it.
        foreach (Hole h in Sub1.Keys)
        {
            if (Sub1[h] == term1 && Sub2[h] == term2)
                return h;
        }

        // Case four: just make a new hole.
        Hole nh = hm.MakeHole();
        Sub1.Add(nh, term1);
        Sub2.Add(nh, term2);
        return nh;
    }
}