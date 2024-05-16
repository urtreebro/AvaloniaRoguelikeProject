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


    public Player Player
    {
        get { return Field.Player; }
    }

    protected override void Tick()
    {
        SetPlayerMovingTarget();

        //SetEnemyMovingTarget();

        DoGameObjectsLogic();

        MoveGameObjects();

        if (Player.CellLocation.ToPoint() == Field.Exit.Location)
        {
            Field = new(Lvl);
        }

        if (Player.IsNewLvl())
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
        foreach (var enemy in Field.GameObjects.OfType<Enemy>())
        {
            if (!enemy.IsMoving)
            {
                if (!enemy.SetTarget(enemy.Facing))
                {
                    if (!enemy.SetTarget((Facing)_rnd.Next(4)))
                    {
                        enemy.SetTarget(null);
                    }
                }
            }
        }
    }

    private void DoGameObjectsLogic()
    {
        var currentObjects = Field.GameObjects.OfType<MovingGameObject>().Except(new MovingGameObject[] { Player }).ToList();
        foreach (var obj in currentObjects)
        {
            obj.DoMainLogicEachGameTick();
        }
    }

    private bool IsGameRunning()
    {
        return Player.IsAlive();
    }

    private void LvlUp()
    {
        Lvl++;
        Player.LvlUp();
    }
}