using System;

namespace AvaloniaRoguelike.Model
{
    public class Player : MovingGameObject
    {
        public Player(GameField field, CellLocation location, Facing facing) : base(field, location, facing, 20, 4, 1) { }
    }
}
