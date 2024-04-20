using System;

namespace AvaloniaRoguelike.Model
{
    
    public class Player : MovingGameObject
    {
        protected override double SpeedFactor => _speed * base.SpeedFactor;
        public Player(GameField field, CellLocation location, Facing facing, int hp, int attack, double speed) : base(field, location, facing, hp, attack, speed) { }
    }
}
