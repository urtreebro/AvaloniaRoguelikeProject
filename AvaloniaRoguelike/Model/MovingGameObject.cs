using System;
using System.Linq;
using ReactiveUI;

namespace AvaloniaRoguelike.Model
{
    public abstract class MovingGameObject : GameObject
    {
        private GameField _field;
        private Facing _facing;
        private CellLocation _cellLocation;
        private CellLocation _targetCellLocation;

        protected MovingGameObject(
            GameField field,
            CellLocation location,
            Facing facing)
            : base(location.ToPoint())
        {
            _field = field;
            Facing = facing;
            CellLocation = TargetCellLocation = location;
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

        public void MoveToTarget()
        {
            if (TargetCellLocation == CellLocation)
                return;

            // TODO: extract to methods 
            var speed = GameField.CellSize *
                        (_field.Tiles[CellLocation.X, CellLocation.Y].Speed +
                         _field.Tiles[TargetCellLocation.X, TargetCellLocation.Y].Speed) / 2
                        * SpeedFactor;
            var pos = Location;
            var direction = GetDirection(CellLocation, TargetCellLocation);
            if (direction == Facing.North)
            {
                pos = pos.WithY(pos.Y - speed);
                Location = pos;
                if (pos.Y / GameField.CellSize <= TargetCellLocation.Y)
                    SetLocation(TargetCellLocation);
            }
            else if (direction == Facing.South)
            {
                pos = pos.WithY(pos.Y + speed);
                Location = pos;
                if (pos.Y / GameField.CellSize >= TargetCellLocation.Y)
                    SetLocation(TargetCellLocation);
            }
            else if (direction == Facing.West)
            {
                pos = pos.WithX(pos.X - speed);
                Location = pos;
                if (pos.X / GameField.CellSize <= TargetCellLocation.X)
                    SetLocation(TargetCellLocation);
            }
            else if (direction == Facing.East)
            {
                pos = pos.WithX(pos.X + speed);
                Location = pos;
                if (pos.X / GameField.CellSize >= TargetCellLocation.X)
                    SetLocation(TargetCellLocation);
            }
        }

        private bool SetTarget(CellLocation loc)
        {
            if (IsMoving)
                //We are the bear rolling from the hill
                throw new InvalidOperationException("Unable to change direction while moving");
            if (loc == CellLocation)
                return true;
            Facing = GetDirection(CellLocation, loc);
            if (loc.X < 0 || loc.Y < 0) // IsLocationOutOfBounds 
                return false;
            if (loc.X >= _field.Width || loc.Y >= _field.Height) // IsLocationOutOfBounds
                return false;
            if (!_field.Tiles[loc.X, loc.Y].IsPassable) // bool IsTilePassable
                return false;

            if (
                _field.GameObjects.OfType<MovingGameObject>()
                    .Any(t => t != this && (t.CellLocation == loc || t.TargetCellLocation == loc))) // bool CheckIfDestinationMatchesTarget?
                return false;

            TargetCellLocation = loc;
            return true;
        }

        private CellLocation GetTileAtDirection(Facing facing)
        {
            //if (facing == Facing.North)
            //    return CellLocation.WithY(CellLocation.Y - 1);
            //if (facing == Facing.South)
            //    return CellLocation.WithY(CellLocation.Y + 1);
            //if (facing == Facing.West)
            //    return CellLocation.WithX(CellLocation.X - 1);
            //return CellLocation.WithX(CellLocation.X + 1);

            return facing switch
            {
                Facing.North => CellLocation.WithY(CellLocation.Y - 1),
                Facing.South => CellLocation.WithY(CellLocation.Y + 1),
                Facing.West => CellLocation.WithX(CellLocation.X - 1),
                Facing.East => CellLocation.WithX(CellLocation.X + 1),
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
}
