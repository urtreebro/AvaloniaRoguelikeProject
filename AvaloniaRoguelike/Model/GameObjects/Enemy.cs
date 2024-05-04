using Avalonia;

namespace AvaloniaRoguelike.Model
{
    public abstract class Enemy : AliveGameObject, IEnemy
    {
        public int EnemyLvl { get; protected set; }
        public Enemy(GameField field, CellLocation location, Facing facing, int enemylvl) : base(field, location, facing) 
        {
            EnemyLvl = enemylvl;
            HP = GetHpByLvl();
            Attack = GetAttackByLvl();
            Speed = GetSpeedByLvl();
        }

        protected override double SpeedFactor => Speed * base.SpeedFactor;

        protected abstract int GetHpByLvl();
        protected abstract int GetAttackByLvl();
        protected abstract double GetSpeedByLvl();
    }
}
