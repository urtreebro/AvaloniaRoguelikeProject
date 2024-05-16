using System;
namespace AvaloniaRoguelike.Model
{
    public class Scarab : Enemy
    {
        public Scarab(GameField field, CellLocation location, Facing facing, int enemylvl) : base(field, location, facing, enemylvl) { }

        protected override int GetHpByLevel()
        {
            return 3 * EnemyLvl + 6;
        }

        protected override int GetAttackByLevel()
        {
            return 3 * EnemyLvl + 1;
        }
        protected override double GetSpeedByLevel()
        {
            return 0.02 * EnemyLvl + 1.2;
        }
    }
}
