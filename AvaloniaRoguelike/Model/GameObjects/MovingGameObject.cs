using System;
using System.Collections.Generic;
using System.Linq;

using AvaloniaRoguelike.Services;

using ReactiveUI;

namespace AvaloniaRoguelike.Model;

public abstract class MovingGameObject : GameObject
{
    protected GameField _field;
    private Facing _facing;
    private CellLocation _cellLocation;
    private CellLocation _targetCellLocation;
    protected MovingGameObject _targetMovingGameObject;

    protected int _sightRadius = 3;
    protected int _attackRadius = 1;
    protected TimeSpan _lastAttackTime = DateTime.Now.TimeOfDay;
    protected TimeSpan _attackCooldown = TimeSpan.FromSeconds(2);

    protected int _health;
    private readonly IPathFindingService _pathFindingService;

    public MovingGameObject(
        GameField field,
        CellLocation location,
        Facing facing)
        : base(location.ToPoint())
    {
        _field = field;
        Facing = facing;
        CellLocation = TargetCellLocation = location;
        _pathFindingService = new AStarPathFindingService();
    }

    public override int Layer => 1;

    public Facing Facing
    {
        get { return _facing; }
        set
        {
            this.RaiseAndSetIfChanged(ref _facing, value);
        }
    }

    public int Health
    {
        get { return _health; }
        set
        {
            this.RaiseAndSetIfChanged(ref _health, value);
        }
    }

    public override CellLocation CellLocation
    {
        get { return _cellLocation; }
        protected set
        {
            this.RaiseAndSetIfChanged(ref _cellLocation, value);
            this.RaiseAndSetIfChanged(ref _cellLocation, value, nameof(IsMoving));
            Location = CellLocation.ToPoint();
        }
    }

    public CellLocation TargetCellLocation
    {
        get { return _targetCellLocation; }
        private set
        {
            this.RaiseAndSetIfChanged(ref _targetCellLocation, value);
            this.RaiseAndSetIfChanged(ref _targetCellLocation, value, nameof(IsMoving));
        }
    }

    public bool IsMoving => TargetCellLocation != CellLocation;

    protected virtual double SpeedFactor => (double)1 / 15; // TODO: Read from config

    public virtual void DoMainLogicEachGameTick()
    {
        // Получить все объекты в поле зрения
        var tiles = GetTilesAtSight();
        // Если среди них есть противник (игрок) - идти к игроку
        var isEnemyInRange = CheckIfEnemyInRange(tiles);
        // Если игрок на соседней клетке или достижим атакой с текущей - атаковать с заданной частотой
        //var canAttackEnemy = CheckCanAttackEnemy();

        // противник в зоне видимости, но нельзя атаковать - пытаться идти к нему
        // противник в зоне видимости и можно его атаковать - атаковать
        if (isEnemyInRange)
        {
            // идти к цели
            if (!IsMoving)
            {
                SetTarget(_field.Player.CellLocation);
            }
            //TargetCellLocation = _field.Player.CellLocation;
            _targetMovingGameObject = _field.Player;
            //var path = _pathFindingService.FindPath(_field, CellLocation, TargetCellLocation);
            //if (path == null)
            //{
            //    // TODO: MakarovEA, log.Debug?
            //    return;
            //}
            return;
        }
        else
        {
            _targetMovingGameObject = null;
            // Иначе случайно бродить
            if (!IsMoving)
            {
                SetTarget(GetRandomFacing());
            }
        }
    }

    public bool SetTarget(Facing? facing)
        => SetTarget(facing.HasValue ? GetTileAtDirection(facing.Value) : CellLocation);

    private double GetSpeed()
    {
        return GameField.CellSize *
                    (_field[CellLocation.X, CellLocation.Y].Speed +
                     _field[TargetCellLocation.X, TargetCellLocation.Y].Speed) / 2
                    * SpeedFactor;
    }

    protected Facing GetRandomFacing()
    {
        return (Facing)Random.Shared.Next(4);
    }

    protected TerrainTile[] GetTilesAtSight()
    {
        return _field.GetTilesAtSight(CellLocation, _sightRadius);
    }

    protected bool CheckIfEnemyInRange(TerrainTile[] tiles)
    {
        return tiles.Any(t => t.CellLocation == _field.Player.CellLocation);
    }

    //TODO: https://faronbracy.github.io/RogueSharp/ need to edit brenches, cause Map needed and, maybe, redo pathfinding using roguesharp library
    public void MoveToTarget()
    {
        if (TargetCellLocation == CellLocation)
        {
            return;
        }

        var path = _pathFindingService.FindPath(_field, CellLocation, TargetCellLocation);
        if (path == null)
        {
            // TODO: MakarovEA, log.Debug?
            return;
        }
        var nextPathCell = path.Skip(1).First().Position;
        var direction = GetDirection(CellLocation, nextPathCell);
        var speed = GetSpeed();

        var movingRules = new Dictionary<Facing, Action>
        {
            {
                Facing.North, () =>
                {
                    Location = Location.WithY(Location.Y - speed);
                    if (Location.Y / GameField.CellSize <= TargetCellLocation.Y)
                        CellLocation = TargetCellLocation;
                } 
            },
            {
                Facing.South, () =>
                {
                    Location = Location.WithY(Location.Y + speed);
                    if (Location.Y / GameField.CellSize >= TargetCellLocation.Y)
                        CellLocation = TargetCellLocation;
                }
            },
            {
                Facing.West, () => 
                {
                    Location = Location.WithX(Location.X - speed);
                    if (Location.X / GameField.CellSize <= TargetCellLocation.X)
                        CellLocation = TargetCellLocation;
                }
            },
            {
                Facing.East, () =>
                {
                    Location = Location.WithX(Location.X + speed);
                    if (Location.X / GameField.CellSize >= TargetCellLocation.X)
                        CellLocation = TargetCellLocation;
                }
            }
        };
        movingRules[direction]();
    }


    protected bool SetTarget(CellLocation loc)
    {
        if (IsMoving)
            throw new InvalidOperationException("Unable to change direction while moving");

        if (loc == CellLocation) 
            return true;

        Facing = GetDirection(CellLocation, loc);

        if (IsLocationOutOfBounds(loc)) 
            return false;

        if (!IsTilePassable(loc)) 
            return false;

        if (CheckIfDestinationMatchesTarget(loc))
            return false;

        TargetCellLocation = loc;
        return true;
    }

    private bool CheckIfDestinationMatchesTarget(CellLocation loc)
    {
        return _field.GameObjects.OfType<MovingGameObject>()
                .Any(t => t != this && t.TargetCellLocation == loc);
    }
    //private bool CheckIfDestinationMatchesTarget(CellLocation loc)
    //{
    //    return _field.GameObjects.OfType<MovingGameObject>()
    //            .Any(t => t != this && (t.CellLocation == loc || t.TargetCellLocation == loc));
    //}

    private bool IsTilePassable(CellLocation loc)
    {
        return _field[loc.X, loc.Y].IsPassable;
    }

    private bool IsLocationOutOfBounds(CellLocation loc)
    {
        return (loc.X < 0 || loc.Y < 0) || (loc.X >= _field.Width || loc.Y >= _field.Height);
    }

    private CellLocation GetTileAtDirection(Facing facing)
    {
        return facing switch
        {
            // TODO: MakarovEA, не создавать новую клетку, а получать нужную клетку карты по координатам, сравнить
            Facing.North => _field[CellLocation.X, CellLocation.Y - 1].CellLocation, //CellLocation.WithY(CellLocation.Y - 1),
            Facing.South => _field[CellLocation.X, CellLocation.Y + 1].CellLocation,
            Facing.West => _field[CellLocation.X - 1, CellLocation.Y].CellLocation,
            Facing.East => _field[CellLocation.X + 1, CellLocation.Y].CellLocation,
            _ => throw new NotImplementedException(),
        };
    }

    private Facing GetDirection(CellLocation current, CellLocation target)
    {
        // TODO: to switch expression?
        if (target.X < current.X)
            return Facing.West;
        if (target.X > current.X)
            return Facing.East;
        if (target.Y < current.Y)
            return Facing.North;
        return Facing.South;
    }
}
