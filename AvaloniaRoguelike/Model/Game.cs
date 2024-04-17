using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia.Input;

namespace AvaloniaRoguelike.Model
{
    public class Game : GameBase
    {
        private GameField _field;
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

        public GameField Field
        {
            get => _field;
            set => _field = value;
        }

        protected override void Tick()
        {
            SetPlayerMovingTarget();

            MoveGameObjects();

            if (_field.Player.CellLocation.ToPoint() == _field.Exit.Location)
            {
                _field = new();
            }
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
        }
    }
}
