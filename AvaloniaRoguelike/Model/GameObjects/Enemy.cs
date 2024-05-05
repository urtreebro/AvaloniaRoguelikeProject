namespace AvaloniaRoguelike.Model;

public abstract class Enemy : AliveGameObject, IEnemy
{
    public Enemy(
        GameField field,
        CellLocation location,
        Facing facing,
        int hp,
        int attack,
        double speed)
        : base(field, location, facing, hp, attack, speed) 
    { }

    protected override double SpeedFactor => _speed * base.SpeedFactor;
    protected abstract int GetHpByLvl();
    protected abstract int GetAttackByLvl();
    protected abstract double GetSpeedByLvl();
}