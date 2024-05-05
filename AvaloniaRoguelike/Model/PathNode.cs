namespace AvaloniaRoguelike.Model;

public sealed class PathNode
{
    public PathNode(CellLocation position)
    {
        Position = position;
    }
    public PathNode(CellLocation position, PathNode from)
    {
        Position = position;
        CameFrom = from;
    }

    /// <summary>
    /// Координаты точки на карте.
    /// </summary>
    public CellLocation Position { get; set; }

    /// <summary>
    /// Длина пути от старта (G).
    /// </summary>
    public int PathLengthFromStart { get; set; }

    /// <summary>
    /// Точка, из которой пришли в эту точку.
    /// </summary>
    public PathNode CameFrom { get; set; }

    /// <summary>
    /// Примерное расстояние до цели (H).
    /// </summary>
    public int HeuristicEstimatePathLength { get; set; }

    /// <summary>
    /// Ожидаемое полное расстояние до цели (F).
    /// </summary>
    public int EstimateFullPathLength => PathLengthFromStart + HeuristicEstimatePathLength;
}