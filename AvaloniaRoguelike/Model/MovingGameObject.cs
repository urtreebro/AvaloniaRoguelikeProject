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

        //stats
        public int Hp{
            get;
            protected set;
        }
        public int Attack{
            get;
            protected set;
        }
        public double _speed;

        protected MovingGameObject(
            GameField field,
            CellLocation location,
            Facing facing,
            int hp,
            int attack,
            double speed)
            : base(location.ToPoint()),
            
        {
            _field = field;
            Facing = facing;
            CellLocation = TargetCellLocation = location;
            Hp = hp;
            Attack = attack;
            _speed = speed;

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

        private double GetSpeed()
        {
            double speed = GameField.CellSize *
                        (_field.Tiles[CellLocation.X, CellLocation.Y].Speed +
                         _field.Tiles[TargetCellLocation.X, TargetCellLocation.Y].Speed) / 2
                        * SpeedFactor;
            return speed;
        }

        public void MoveToTarget()
        {
            if (TargetCellLocation == CellLocation)
                return;

            // TODO: extract to methods 
            var speed = GetSpeed();
            var pos = Location;
            var direction = GetDirection(CellLocation, TargetCellLocation);

            switch (direction)
            {
                case Facing.North:
                    pos = pos.WithY(pos.Y - speed);
                    Location = pos;
                    if (pos.Y / GameField.CellSize <= TargetCellLocation.Y)
                        SetLocation(TargetCellLocation);
                    break;
                case Facing.South:
                    pos = pos.WithY(pos.Y + speed);
                    Location = pos;
                    if (pos.Y / GameField.CellSize >= TargetCellLocation.Y)
                        SetLocation(TargetCellLocation);
                    break;
                case Facing.West:
                    pos = pos.WithX(pos.X - speed);
                    Location = pos;
                    if (pos.X / GameField.CellSize <= TargetCellLocation.X)
                        SetLocation(TargetCellLocation);
                    break;
                case Facing.East:
                    pos = pos.WithX(pos.X + speed);
                    Location = pos;
                    if (pos.X / GameField.CellSize >= TargetCellLocation.X)
                        SetLocation(TargetCellLocation);
                    break;
            }
            //if (direction == Facing.North)
            //{
            //    pos = pos.WithY(pos.Y - speed);
            //    Location = pos;
            //    if (pos.Y / GameField.CellSize <= TargetCellLocation.Y)
            //        SetLocation(TargetCellLocation);
            //}
            //else if (direction == Facing.South)
            //{
            //    pos = pos.WithY(pos.Y + speed);
            //    Location = pos;
            //    if (pos.Y / GameField.CellSize >= TargetCellLocation.Y)
            //        SetLocation(TargetCellLocation);
            //}
            //else if (direction == Facing.West)
            //{
            //    pos = pos.WithX(pos.X - speed);
            //    Location = pos;
            //    if (pos.X / GameField.CellSize <= TargetCellLocation.X)
            //        SetLocation(TargetCellLocation);
            //}
            //else if (direction == Facing.East)
            //{
            //    pos = pos.WithX(pos.X + speed);
            //    Location = pos;
            //    if (pos.X / GameField.CellSize >= TargetCellLocation.X)
            //        SetLocation(TargetCellLocation);
            //}
        }

        private bool SetTarget(CellLocation loc)
        {
            if (IsMoving) throw new InvalidOperationException("Unable to change direction while moving");

            if (loc == CellLocation) return true;

            Facing = GetDirection(CellLocation, loc);

            if (IsLocationOutOfBounds(loc)) return false;

            if (!IsTilePassable(loc)) return false;

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



        //A* pathfinding
        public static List<Point> FindPath(int[,] field, Point start, Point goal)
        {
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();

            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
            };
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.OrderBy(node =>
                  node.EstimateFullPathLength).First();

                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
                {
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node =>
                      node.Position == neighbourNode.Position);

                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                      if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                    {
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                    }
                }
            }
            return null;
        }

        private static int GetDistanceBetweenNeighbours()
        {
            //можно переписать в зависимости от ландшафта
            return 1;
        }

        private static int GetHeuristicPathLength(Point from, Point to)
        {
            return Convert.ToInt32(Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y));
        }

        private static Collection<PathNode> GetNeighbours(PathNode pathNode, Point goal, int[,] field)
        {
            var result = new Collection<PathNode>();

            // Соседними точками являются соседние по стороне клетки.
            Point[] neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

            foreach (var point in neighbourPoints)
            {
                // Проверяем, что не вышли за границы карты.
                if (point.X < 0 || point.X >= field.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= field.GetLength(1))
                    continue;
                // Проверяем, что по клетке можно ходить.
                if ((field[(int)point.X, (int)point.Y] != 0) && (field[(int)point.X, (int)point.Y] != 1))
                    continue;
                // Заполняем данные для точки маршрута.
                var neighbourNode = new PathNode()
                {
                    Position = point,
                    CameFrom = pathNode,
                    PathLengthFromStart = pathNode.PathLengthFromStart +
                    GetDistanceBetweenNeighbours(),
                    HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                };
                result.Add(neighbourNode);
            }
            return result;
        }

        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            return result;
        }
        
    }

    public class PathNode
    {
        // Координаты точки на карте.
        public Point Position { get; set; }
        // Длина пути от старта (G).
        public int PathLengthFromStart { get; set; }
        // Точка, из которой пришли в эту точку.
        public PathNode CameFrom { get; set; }
        // Примерное расстояние до цели (H).
        public int HeuristicEstimatePathLength { get; set; }
        // Ожидаемое полное расстояние до цели (F).
        public int EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
    }
}
