using System.Collections.Generic;
using System.Linq;

using Avalonia.Input;

namespace AvaloniaRoguelike.Model
{
    public class Game : GameBase
    {
        private readonly GameField _field;
        private readonly Dictionary<Key, Facing> _keyFacingPairs = new Dictionary<Key, Facing>
        {
            { Key.W, Facing.North},
            { Key.S, Facing.South},
            { Key.A, Facing.West},
            { Key.D, Facing.East}
        };

        public Game(GameField field)
        {
            _field = field;
        }

        protected override void Tick()
        {
            SetPlayerMovingTarget();

            MoveGameObjects();
        }

        private void MoveGameObjects()
        {
            foreach (var obj in _field.GameObjects.OfType<MovingGameObject>())
            {
                obj.MoveToTarget();
            }
        }

        private void SetPlayerMovingTarget()
        {
            if (_field.Player.IsMoving)
            {
                return;
            }
            if (_keyFacingPairs.TryGetValue(Keyboard.LastKeyPressed(), out var facing))
            {
                _field.Player.SetTarget(facing);
            }
            //if (Keyboard.IsKeyDown(Key.W))
            //    _field.Player.SetTarget(Facing.North);
            //else if (Keyboard.IsKeyDown(Key.S))
            //    _field.Player.SetTarget(Facing.South);
            //else if (Keyboard.IsKeyDown(Key.A))
            //    _field.Player.SetTarget(Facing.West);
            //else if (Keyboard.IsKeyDown(Key.D))
            //    _field.Player.SetTarget(Facing.East);
        }
    }
}
