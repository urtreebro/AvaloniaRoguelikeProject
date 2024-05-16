using Avalonia;

namespace AvaloniaRoguelike.Model
{
    public abstract class Enemy : AliveGameObject, IEnemy
    {
        public int EnemyLvl { get; protected set; }
        public Enemy(GameField field, CellLocation location, Facing facing, int enemylvl) : base(field, location, facing)
        {
            EnemyLvl = enemylvl;
            HP = GetHpByLevel();
            Attack = GetAttackByLevel();
            Speed = GetSpeedByLevel();
        }

        protected override double SpeedFactor => Speed * base.SpeedFactor;

        protected abstract int GetHpByLevel();
        protected abstract int GetAttackByLevel();
        protected abstract double GetSpeedByLevel();
    }
}
