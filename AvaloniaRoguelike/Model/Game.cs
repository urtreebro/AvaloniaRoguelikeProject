using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Avalonia.Input;
using System;

namespace AvaloniaRoguelike.Model;

public class Game : GameBase
{
    private GameField _field;
    private Camera _camera;
    private readonly Dictionary<Key, Facing> _keyFacingPairs = new()
    {
        { Key.W, Facing.North},
        { Key.S, Facing.South},
        { Key.A, Facing.West},
        { Key.D, Facing.East}
    };

    public Game(GameField field)
    {
        _field = field;
        _camera = new Camera(_field);
    }

    public GameField Field
    {
        get { return _field; }
        set
        {
            this.RaiseAndSetIfChanged(ref _field, value);
        }
    }

    public Camera Camera
    {
        get { return _camera; }
        set
        {
            this.RaiseAndSetIfChanged(ref _camera, value);
        }
    }


    public Player Player
    {
        get { return Field.Player; }
    }

    protected override void Tick()
    {
        CheckLastKeyPressed();

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

    private void MoveCamera()
    {
        Camera.ReCalculateVisibleObjects(Player.CellLocation);
    }

    private void CheckLastKeyPressed()
    {
        var key = Keyboard.LastKeyPressed();
        switch (key)
        {
            case Key.W:
            case Key.A:
            case Key.S:
            case Key.D:
                SetPlayerMovingTarget(key);
                MoveCamera();
                break;
            case Key.I:
                // TODO: inventory view
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

    private void SetPlayerMovingTarget(Key key)
    {
        if (Field.Player.IsMoving)
        {
            return;
        }
        Field.Player.SetTarget(_keyFacingPairs[key]);
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