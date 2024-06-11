using System;
using System.Collections.ObjectModel;
using System.Linq;

using AvaloniaRoguelike.Services;
using AvaloniaRoguelike.ViewModels;

using ReactiveUI;

namespace AvaloniaRoguelike.Model;

public class GameField : ViewModelBase
{
    public static GameField DesignInstance { get; } = new GameField(0);

    private readonly TerrainTile[,] _map;
    private readonly IMapGeneratingService _mapGeneratingService;
    private ObservableCollection<GameObject> _gameObjects;

    public const double CellSize = 32;
    public const int Default_Width = 32;
    public const int Default_Height = 24;

    public GameField(int lvl) : this(Default_Width, Default_Height, lvl) { }

    public GameField(int width, int height, int lvl = 1)
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
        GameObjects.Add(Exit = new Exit(new CellLocation(GetPassableCoords()).ToPoint()));

        var enemiesCount = Random.Next(2, Lvl * 2 + 1);
        for (var c = 0; c < enemiesCount; c++)
        {
            GameObjects.Add(GetRandomEnemy());
        }
    }

    public void AddPlayer(Player player)
    {
        Player = player;
        Player.CellLocation = new CellLocation(GetPassableCoords());
        Player.TargetCellLocation = Player.CellLocation;
        GameObjects.Add(Player);
    }

    public TerrainTile this[int x, int y]
    {
        get => _map[x, y];
        set => _map[x, y] = value;
    }

    public GameObject this[int index]
    {
        get => GameObjects[index];
    }

    public TerrainTile this[CellLocation location] => _map[location.X, location.Y];

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<GameObject> GameObjects
    {
        get { return _gameObjects; }
        set
        {
            this.RaiseAndSetIfChanged(ref _gameObjects, value);
        }
    }

    public int Lvl { get; }

    public Random Random { get; } = new();

    public Player Player { get; private set; }

    public Exit Exit { get; }

    public int Height { get; }

    public int Width { get; }

    public TerrainTile[] GetTilesAtSight(CellLocation cell, int sightRadius)
    {
        return _map
            .Cast<TerrainTile>()
            .Where(tile => tile.IsPassable && tile.IsInRange(cell, sightRadius))
            .ToArray();
    }

    public (int, int) GetPassableCoords()
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


    // TODO: MakarovEA, вынести в игру или в отдельный сервис
    private Facing GetRandomFacing()
    {
        return (Facing)Random.Next(4);
    }

    private Enemy GetRandomEnemy()
    {
        if (Random.Next(0, 2) == 1)
        {
            return new Mummy(this, new CellLocation(GetPassableCoords()), GetRandomFacing(), Lvl);
        }
        return new Scarab(this, new CellLocation(GetPassableCoords()), GetRandomFacing(), Lvl);
    }
}