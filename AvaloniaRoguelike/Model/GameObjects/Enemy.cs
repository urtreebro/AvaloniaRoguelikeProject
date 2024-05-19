namespace AvaloniaRoguelike.Model;

public abstract class Enemy : AliveGameObject, IEnemy
{
    public Enemy(
        GameField field,
        CellLocation location,
        Facing facing,
        int enemylvl)
        : base(field, location, facing)
    {
        EnemyLvl = enemylvl;
        Health = GetHealthByLevel();
        Damage = GetDamageByLevel();
        Speed = GetSpeedByLevel();
    }

    public int EnemyLvl { get; protected set; }

    protected override double SpeedFactor => Speed * base.SpeedFactor;
    protected abstract int GetHealthByLevel();
    protected abstract int GetDamageByLevel();
    protected abstract double GetSpeedByLevel();
}