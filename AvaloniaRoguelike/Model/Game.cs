using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Avalonia.Input;

namespace AvaloniaRoguelike.Model;

public class Game : GameBase
{
    private GameField _field;
    private Player _player;
    private Camera _camera;
    private bool _playerViewVisible = false;
    private int _playerViewWidth = 0;
    private readonly Dictionary<Key, Facing> _keyFacingPairs = new()
    {
        { Key.W, Facing.North},
        { Key.S, Facing.South},
        { Key.A, Facing.West},
        { Key.D, Facing.East}
    };

    public Game()
    {
        _camera = new Camera();
        Field = new GameField(0);
        var startCoordinates = Field.GetPassableCoords();
        _player = new Player(new CellLocation(startCoordinates));
        _player.SetField(Field);
        Field.AddPlayer(_player);
        MoveCamera();
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

    public bool PlayerViewVisible
    {
        get { return _playerViewVisible; }
        set
        {
            this.RaiseAndSetIfChanged(ref _playerViewVisible, value);
        }
    }

    public int PlayerViewWidth
    {
        get { return _playerViewWidth; }
        set
        {
            this.RaiseAndSetIfChanged(ref _playerViewWidth, value);
        }
    }

    public Player Player
    {
        get { return Field.Player; }
    }

    public void SetPlayerName(string name)
    {
        _player.Name = name;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Stop()
    {
        base.Stop();
    }

    public void OpenClosePlayerView()
    {
        PlayerViewVisible = !PlayerViewVisible;
        PlayerViewWidth = PlayerViewWidth == 300
            ? 0
            : 300;
        MoveCamera();
    }

    protected override void Tick()
    {
        CheckLastKeyPressed();

        DoGameObjectsLogic();

        MoveGameObjects();

        CheckIsDead();

        if (Player.CellLocation.ToPoint() == Field.Exit.Location)
        {
            //Field.RemovePlayer();
            Field = new(Lvl);
            _player.SetField(Field);
            Field.AddPlayer(_player);
            MoveCamera();
        }

        //if (Player.IsNewLvl())
        //{
        //    LvlUp();
        //}
    }

    private void MoveCamera()
    {
        Camera.MoveCamera(Player.Location);
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