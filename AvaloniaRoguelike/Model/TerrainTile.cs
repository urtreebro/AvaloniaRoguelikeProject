using System;
using System.Collections.Generic;
using Avalonia;

namespace AvaloniaRoguelike.Model
{
    public enum TerrainTileType
    {
        Plain, //passable, shoot-thru
        Wall,
        Background
    }

    public class TerrainTile : GameObject
    {
        private static readonly Dictionary<TerrainTileType, double> Speeds = new Dictionary<TerrainTileType, double>
        {
            {TerrainTileType.Plain, 1},
            {TerrainTileType.Wall, 0},
            {TerrainTileType.Background, 0}
        };
        private static readonly Dictionary<TerrainTileType, bool> ShootThrus = new Dictionary<TerrainTileType, bool>
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
    }
}
