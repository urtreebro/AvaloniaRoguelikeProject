using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
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
            CheckLastKeyPressed();

            SetEnemyMovingTarget();

            MoveGameObjects();

            CheckIsDead();

            if (Field.Player.CellLocation.ToPoint() == Field.Exit.Location)
            {
                Field = new(Level);
            }

            if (Field.Player.IsNewLevel())
            {
                LevelUp();
            }
        }

        private void MoveGameObjects()
        {
            foreach (var obj in Field.GameObjects.OfType<MovingGameObject>())
            {
                obj.MoveToTarget();
            }
        }

        private void SetPlayerMovingTarget(Facing facing)
        {
            if (Field.Player.IsMoving)
            {
                return;
            }
            Field.Player.SetTarget(facing);
        }

        private void SetEnemyMovingTarget()
        {
            foreach (var enemy in _field.GameObjects.OfType<Enemy>())
            {
                if (!enemy.IsMoving)
                {
                    if (!enemy.SetTarget(enemy.Facing))
                    {
                        if (!enemy.SetTarget((Facing)rnd.Next(4)))
                            enemy.SetTarget(null);
                    }
                }
            }
        }

        private bool IsGameRunning()
        {
            if (Field.Player.IsAlive()) return true;
            return false;
        }

        private void LevelUp()
        {
            Level++;
            Field.Player.LevelUp();
        }

        private void CheckLastKeyPressed()
        {
            Key key = Keyboard.LastKeyPressed();
            switch (key)
            {
                case Key.W:
                    SetPlayerMovingTarget(Facing.North);
                    break;
                case Key.A:
                    SetPlayerMovingTarget(Facing.West);
                    break;
                case Key.S:
                    SetPlayerMovingTarget(Facing.South);
                    break;
                case Key.D:
                    SetPlayerMovingTarget(Facing.East);
                    break;
                case Key.I:
                    break;
                case Key.X:
                    Attack();
                    break;
                default:
                    break;
            }
        }

        public void Attack()
        {
            CellLocation target = new(Field.Player.CellLocation.X + 1, Field.Player.CellLocation.Y);
            foreach (var enemy in Field.GameObjects.OfType<Enemy>())
            {
                if (enemy.CellLocation == target)
                {
                    enemy.GetDamage(Field.Player.Attack);
                }
            }
        }

        public void RemoveObjectFromField(GameObject obj)
        {
            Field.GameObjects.Remove(obj);
        }

        public void CheckIsDead()
        {
            foreach (var enemy in Field.GameObjects.OfType<Enemy>())
            {
                if (!enemy.IsAlive())
                {
                    RemoveObjectFromField(enemy);
                }
            }
        }
    }
}
