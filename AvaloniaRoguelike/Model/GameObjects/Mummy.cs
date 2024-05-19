namespace AvaloniaRoguelike.Model;

public class Mummy : Enemy
{
    public Mummy(
        GameField field,
        CellLocation location,
        Facing facing,
        int enemylvl)
        : base(field, location, facing, enemylvl)
    { }

    protected override int GetHealthByLevel()
    {
        return 3 * EnemyLvl + 20;
    }

    protected override int GetDamageByLevel()
    {
        return 3 * EnemyLvl + 4;
    }
    protected override double GetSpeedByLevel()
    {
        return 0.02 * EnemyLvl + 0.5;
    }
}