namespace AvaloniaRoguelike.Model;

public class Scarab : Enemy
{
    //public Scarab(
    //    GameField field,
    //    CellLocation location,
    //    Facing facing,
    //    int Health,
    //    int Damage,
    //    double speed)
    //    : base(field, location, facing, 8, 1, 1.8)
    //{ }
    public Scarab(
        GameField field,
        CellLocation location,
        Facing facing,
        int enemylvl) 
        : base(field, location, facing, enemylvl) 
    { }

    protected override int GetHealthByLevel()
    {
        return 3 * EnemyLvl + 6;
    }

    protected override int GetDamageByLevel()
    {
        return 3 * EnemyLvl + 1;
    }
    protected override double GetSpeedByLevel()
    {
        return 0.02 * EnemyLvl + 1.2;
    }
}