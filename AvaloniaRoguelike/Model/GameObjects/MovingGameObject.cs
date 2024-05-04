using System;
using System.Collections.Generic;
using System.Linq;

using AvaloniaRoguelike.Services;

using ReactiveUI;

namespace AvaloniaRoguelike.Model;

public abstract class MovingGameObject : GameObject
{
    private GameField _field;
    private Facing _facing;
    private CellLocation _cellLocation;
    private CellLocation _targetCellLocation;

    private readonly IPathFindingService _pathFindingService;

    public MovingGameObject(
        GameField field,
        CellLocation location,
        Facing facing,
        IPathFindingService pathFindingService)
        : base(location.ToPoint())
    {
        _field = field;
        Facing = facing;
        CellLocation = TargetCellLocation = location;
        _pathFindingService = pathFindingService;
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

    public CellLocation CellLocation
    {
        get { return _cellLocation; }
        private set
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

    protected virtual double SpeedFactor => (double)1 / 12; // TODO: Read from config

    public bool SetTarget(Facing? facing)
        => SetTarget(facing.HasValue ? GetTileAtDirection(facing.Value) : CellLocation);

    private int GetSpeed()
    {
        //double speed = GameField.CellSize *
        //            (_field.Tiles[CellLocation.X, CellLocation.Y].Speed +
        //             _field.Tiles[TargetCellLocation.X, TargetCellLocation.Y].Speed) / 2
        //            * SpeedFactor;
        //return speed;
        return 1;
    }

    public void MoveToTarget()
    {
        if (TargetCellLocation == CellLocation)
            return;

        var path = _pathFindingService.FindPath(_field.Map, CellLocation, TargetCellLocation);
        var nextPathCell = path.First().Position;
        var direction = GetDirection(CellLocation, nextPathCell);
        var speed = GetSpeed();

        CellLocation = GetNewCellLocation(direction, nextPathCell, speed);
    }

    private CellLocation GetNewCellLocation(
        Facing direction,
        CellLocation nextPathCell,
        int speed)
    {
        var movingRules = new Dictionary<Facing, Func<bool>>
        {
            {Facing.North, () => CellLocation.Y - speed <= nextPathCell.Y },
            {Facing.South, () => CellLocation.Y + speed >= nextPathCell.Y },
            {Facing.West, () => CellLocation.X - speed <= nextPathCell.X },
            {Facing.East, () => CellLocation.X + speed >= nextPathCell.X }
        };

        if (movingRules[direction]())
            return nextPathCell;
        return CellLocation;
    }


    private bool SetTarget(CellLocation loc)
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
                .Any(t => t != this && (t.CellLocation == loc || t.TargetCellLocation == loc));
    }

    private bool IsTilePassable(CellLocation loc)
    {
        return _field.Tiles[loc.X, loc.Y].IsPassable;
    }

    private bool IsLocationOutOfBounds(CellLocation loc)
    {
        return (loc.X < 0 || loc.Y < 0) || (loc.X >= _field.Width || loc.Y >= _field.Height);
    }

    private CellLocation GetTileAtDirection(Facing facing)
    {
        return facing switch
        {
            // TODO: evma, не создавать новую клетку, а получать нужную клетку карты по координатам
            Facing.North => CellLocation.WithY(CellLocation.Y - 1),
            Facing.South => CellLocation.WithY(CellLocation.Y + 1),
            Facing.West => CellLocation.WithX(CellLocation.X - 1),
            Facing.East => CellLocation.WithX(CellLocation.X + 1),
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

    private void SetLocation(CellLocation loc)
    {
        CellLocation = loc;
        Location = loc.ToPoint();
    }
}