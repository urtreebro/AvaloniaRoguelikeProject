using System;
namespace AvaloniaRoguelike.Model
{
    public class Mummy : Enemy
    {
        public Mummy(GameField field, CellLocation location, Facing facing) : base(field, location, facing, 20, 3, 0.5) { }

    }
}
