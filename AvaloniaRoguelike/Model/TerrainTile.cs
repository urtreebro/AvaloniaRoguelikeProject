using System.Collections.Generic;
using Avalonia;

namespace AvaloniaRoguelike.Model;

public class TerrainTile : GameObject
{
    // TODO: evma, TerrainTileType - много соответствий. Класс?
    private static readonly Dictionary<TerrainTileType, double> Speeds = new()
    {
        {TerrainTileType.Plain, 1},
        {TerrainTileType.Wall, 0},
        {TerrainTileType.Background, 0}
    };
    private static readonly Dictionary<TerrainTileType, bool> ShootThrus = new()
    {
        {TerrainTileType.Plain, true},
        {TerrainTileType.Wall, false},
        {TerrainTileType.Background, false},
    };

    public double Speed => Speeds[Type];
    public bool ShootThru => ShootThrus[Type];
    public bool IsPassable => Speed > 0.1;
    public TerrainTileType Type { get; set; }

    public TerrainTile(Point location, TerrainTileType type) : base(location)
    {
        Type = type;
    }
    public TerrainTile(Point location) : base(location)
    {
        Type = TerrainTileType.Background;
    }
    public TerrainTile(CellLocation cellLocation) : base(cellLocation)
    {
        Type = TerrainTileType.Background;
    }
}