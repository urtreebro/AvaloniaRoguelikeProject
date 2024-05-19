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
        CheckLastKeyPressed();

        //SetPlayerMovingTarget();

        //SetEnemyMovingTarget();

        DoGameObjectsLogic();

        MoveGameObjects();

        CheckIsDead();

        if (Player.CellLocation.ToPoint() == Field.Exit.Location)
        {
            Field = new(Lvl);
        }

        if (Player.IsNewLvl())
        {
            LvlUp();
        }
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
                Field.Player.Attack();
                break;
            default:
                break;
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

    public void RemoveObjectFromField(GameObject obj)
    {
        Field.GameObjects.Remove(obj);
    }
}