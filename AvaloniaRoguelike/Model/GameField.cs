using System;
using Avalonia;
using System.Collections.ObjectModel;
using AvaloniaRoguelike.ViewModels;

namespace AvaloniaRoguelike.Model
{
    public class GameField : ViewModelBase
    {
        public static GameField DesignInstance { get; } = new GameField();
        public const double CellSize = 32;

        public ObservableCollection<GameObject> GameObjects { get; } = new ObservableCollection<GameObject>();

        public TerrainTile[,] Tiles { get; }

        public Map Map { get; } = new Map();

        public Random Random { get; } = new Random();
        public Player Player { get; }
        public Exit Exit { get; }
        public int Height { get; }
        public int Width { get; }

        public GameField() : this(32, 24) { }

        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new TerrainTile[width, height];
            // TODO: Deserialize field
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObjects.Add(
                        Tiles[x, y] =
                            new TerrainTile(new Point(x * CellSize, y * CellSize), GetTypeForCoords(x, y)));
                }
            }
            GameObjects.Add(
                Player = new Player(this, new CellLocation(GetCoords()), Facing.East));
            GameObjects.Add(
                Exit = new Exit(new CellLocation(GetCoords()).ToPoint()));
            GameObjects.Add(
                Enemy = new Enemy(this, new CellLocation(GetCoords()), Facing.East)); //TODO: random facing
        }

        private TerrainTileType GetTypeForCoords(int x, int y)
        {
            if (Map[x, y] == ".") return TerrainTileType.Plain;
            else if (Map[x, y] == "#") return TerrainTileType.Wall;
            return TerrainTileType.Background;
        }

        private (int, int) GetCoords()
        {
            int x = Random.Next(0, Width);
            int y = Random.Next(0, Height);
            while (!(Tiles[x, y].IsPassable))
            {
                x = Random.Next(0, Width);
                y = Random.Next(0, Height);
            }
            return (x, y);
        }
    }
}
