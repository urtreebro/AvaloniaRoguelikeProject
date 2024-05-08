using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Avalonia.Input;

namespace AvaloniaRoguelike.Model;

public class Game : GameBase
{
    private GameField _field;
    private readonly Dictionary<Key, Facing> _keyFacingPairs = new()
    {
        { Key.W, Facing.North},
        { Key.S, Facing.South},
        { Key.A, Facing.West},
        { Key.D, Facing.East}
    };

    private static readonly Random _rnd = new();
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

        SetEnemyMovingTarget();

        MoveGameObjects();

        if (Field.Player.CellLocation.ToPoint() == Field.Exit.Location)
        {
            Field = new(Lvl);
        }

        if (Field.Player.IsNewLvl())
        {
            LvlUp();
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

    private void SetEnemyMovingTarget()
    {
        foreach (var enemy in _field.GameObjects.OfType<Enemy>())
            if (!enemy.IsMoving)
            {
                if (!enemy.SetTarget(enemy.Facing))
                {
                    if (!enemy.SetTarget((Facing)_rnd.Next(4)))
                        enemy.SetTarget(null);
                }
            }
    }

    private bool IsGameRunning()
    {
        return Field.Player.IsAlive();
    }

    private void LvlUp()
    {
        Lvl++;
        Field.Player.LvlUp();
    }
}