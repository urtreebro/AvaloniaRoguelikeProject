namespace AvaloniaRoguelike.Model;

public class Mummy : Enemy
{
    public Mummy(
        GameField field,
        CellLocation location,
        Facing facing)
        : base(field, location, facing, 20, 3, 0.5)
    { }

    protected override int GetHpByLvl()
    {
        return 3 * EnemyLvl + 20;
    }

    protected override int GetAttackByLvl()
    {
        return 3 * EnemyLvl + 4;
    }
    protected override double GetSpeedByLvl()
    {
        return 0.02 * EnemyLvl + 0.5;
    }
}