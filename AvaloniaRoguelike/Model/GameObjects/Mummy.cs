namespace AvaloniaRoguelike.Model;

public class Mummy : Enemy
{
    public Mummy(
        GameField field,
        CellLocation location,
        Facing facing,
        int enemylvl)
        : base(field, location, facing, enemylvl)
    {
        MinDamage = 2;
        MaxDamage = 5;
        Experience = 4;
    }

    protected override int GetHealthByLevel()
    {
        return 3 * EnemyLvl + 20;
    }

    protected override double GetSpeedByLevel()
    {
        return 0.02 * EnemyLvl + 0.5;
    }
}