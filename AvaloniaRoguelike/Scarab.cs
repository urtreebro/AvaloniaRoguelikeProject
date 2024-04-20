using System;
namespace AvaloniaRoguelike.Model
{
	public class Scarab : Enemy
	{
        public Scarab(GameField field, CellLocation location, Facing facing, int hp, int attack, double speed) : base(field, location, facing, 8, 1, 1.8) { }

	}
}

