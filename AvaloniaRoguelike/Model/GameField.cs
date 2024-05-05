using System;
using System.Collections.ObjectModel;

using AvaloniaRoguelike.Services;
using AvaloniaRoguelike.ViewModels;

namespace AvaloniaRoguelike.Model;

public class GameField : ViewModelBase
{
    public static GameField DesignInstance { get; } = new GameField();

    private readonly TerrainTile[,] _map;
    private readonly IMapGeneratingService _mapGeneratingService;

    public const double CellSize = 32;
    public const int Default_Width = 32;
    public const int Default_Height = 24;

    public GameField(int lvl) : this(Default_Width, Default_Height, lvl) { }

    public GameField(IMapGeneratingService mapGeneratingService)
    {
        _mapGeneratingService = mapGeneratingService;
    }

    public GameField(int width, int height, int lvl)
    {
        Width = width;
        Height = height;
        Lvl = lvl;
        GameObjects = [];
        _map = new TerrainTile[Width, Height];
        _mapGeneratingService = new MapGeneratingService();
        _mapGeneratingService.InitializeMap(this);

        // TODO: Deserialize field
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObjects.Add(this[x, y]);
            }
        }
        GameObjects.Add(Player = new Player(this, new CellLocation(GetCoords()), Facing.East));
        GameObjects.Add(Exit = new Exit(new CellLocation(GetCoords()).ToPoint()));

        for (var c = 0; c < 5; c++)
        {
            GameObjects.Add(GetRandomEnemy());
        }
    }

    public TerrainTile this[int x, int y]
    {
        get => _map[x, y];
        set => _map[x, y] = value;
    }

    public TerrainTile this[CellLocation location] => _map[location.X, location.Y];

    public ObservableCollection<GameObject> GameObjects { get; }

    public Random Random { get; } = new();
    public Player Player { get; }
    public Exit Exit { get; }
    public int Height { get; }
    public int Width { get; }

    private (int, int) GetCoords()
    {
        int x = Random.Next(0, Width);
        int y = Random.Next(0, Height);
        while (!(this[x, y].IsPassable))
        {
            x = Random.Next(0, Width);
            y = Random.Next(0, Height);
        }
        return (x, y);
    }

    private Facing GetRandomFacing()
    {
        return (Facing)Random.Next(4);
    }

    private Enemy GetRandomEnemy()
    {
        if (Random.Next(0, 2) == 1)
        {
            return new Mummy(this, new CellLocation(GetCoords()), GetRandomFacing(), Lvl);
        }
        return new Scarab(this, new CellLocation(GetCoords()), GetRandomFacing(), Lvl);
    }
}