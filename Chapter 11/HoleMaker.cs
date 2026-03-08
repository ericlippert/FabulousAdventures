// Fabulous Adventures In Data Structures and Algorithms
// Chapter 11
// Anti-Unification

sealed class HoleMaker
{
    readonly HashSet<Hole> inUse;
    int holenum = 0;
    char holename = 'A';

    public HoleMaker(IEnumerable<Hole> holes)
    {
        this.inUse = new(holes);
    }
    public Hole MakeHole()
    {
        while (true)
        {
            string num = holenum == 0 ? "" : holenum.ToString();
            Hole hole = new Hole($"{holename}{num}");
            if (holename == 'Z')
            {
                holename = 'A';
                holenum++;
            }
            else
                holename++;
            if (!inUse.Contains(hole))
                return hole;
        }
    }
}