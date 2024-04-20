using System;
namespace AvaloniaRoguelike.Model
{
	public abstract class Enemy : MovingGameObject
    {
        protected override double SpeedFactor => _speed * base.SpeedFactor;
	}
}

