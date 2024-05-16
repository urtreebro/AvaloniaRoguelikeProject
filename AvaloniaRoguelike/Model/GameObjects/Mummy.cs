using System;
namespace AvaloniaRoguelike.Model
{
    public class Mummy : Enemy
    {
        public Mummy(GameField field, CellLocation location, Facing facing, int enemylevel) : base(field, location, facing, enemylevel)
        {

        }

        protected override int GetHpByLevel()
        {
            return 3 * EnemyLvl + 20;
        }

        protected override int GetAttackByLevel()
        {
            return 3 * EnemyLvl + 4;
        }
        protected override double GetSpeedByLevel()
        {
            return 0.02 * EnemyLvl + 0.5;
        }

    }
}
