using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

using Avalonia.Input;
using System.Threading.Tasks;

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

        private static Random rnd = new Random();
        public Game(GameField field)
        {
            _field = field;
        }

        public GameField Field
        {
            get { return _field; }
            set
            {
                this.RaiseAndSetIfChanged(ref _field, value);
            }
        }

        protected override void Tick()
        {
            SetPlayerMovingTarget();

            foreach (var enemy in _field.GameObjects.OfType<Enemy>())
                if (!enemy.IsMoving)
                {
                    if (!enemy.SetTarget(enemy.Facing))
                    {
                        if (!enemy.SetTarget((Facing)rnd.Next(4)))
                            enemy.SetTarget(null);
                    }
                }

            MoveGameObjects();

            if (Field.Player.CellLocation.ToPoint() == Field.Exit.Location)
            {
                Field = new();
            }
        }

        private void MoveGameObjects()
        {
            foreach (var obj in Field.GameObjects.OfType<MovingGameObject>())
            {
                obj.MoveToTarget();
            }
        }

        private void SetPlayerMovingTarget()
        {
            if (Field.Player.IsMoving)
            {
                return;
            }
            if (_keyFacingPairs.TryGetValue(Keyboard.LastKeyPressed(), out var facing))
            {
                Field.Player.SetTarget(facing);
            }
        }

        private bool IsGameRunning()
        {
            if (Field.Player.IsAlive()) return true;
            return false;
        }
    }
}
