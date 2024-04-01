using System;
using System.Linq;
using Avalonia.Input;

namespace AvaloniaRoguelike.Model
{
    public class Game : GameBase
    {
        private readonly GameField _field;

        public Game(GameField field)
        {
            _field = field;
        }

        protected override void Tick()
        {
            if (!_field.Player.IsMoving)
            {
                if (Keyboard.IsKeyDown(Key.W))
                    _field.Player.SetTarget(Facing.North);
                else if (Keyboard.IsKeyDown(Key.S))
                    _field.Player.SetTarget(Facing.South);
                else if (Keyboard.IsKeyDown(Key.A))
                    _field.Player.SetTarget(Facing.West);
                else if (Keyboard.IsKeyDown(Key.D))
                    _field.Player.SetTarget(Facing.East);
            }

            foreach (var obj in _field.GameObjects.OfType<MovingGameObject>())
                obj.MoveToTarget();
        }
    }
}
