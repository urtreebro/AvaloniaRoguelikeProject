﻿using System;
using Avalonia;
using AvaloniaRoguelike.Infrastructure;
using System.Collections.ObjectModel;

namespace AvaloniaRoguelike.Model
{
    public class GameField : PropertyChangedBase
    {
        public static GameField DesignInstance { get; } = new GameField();
        public const double CellSize = 32;

        public ObservableCollection<GameObject> GameObjects { get; } = new ObservableCollection<GameObject>();

        public TerrainTile[,] Tiles { get; }

        public Player Player { get; }
        public int Height { get; }
        public int Width { get; }

        public GameField() : this(40, 30) { }

        Random Random { get; } = new Random();

        TerrainTileType GetTypeForCoords(int x, int y)
        {
            return TerrainTileType.Plain;
        }

        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new TerrainTile[width, height];
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
                Player = new Player(this, new CellLocation(width / 2, height / 2), Facing.East));
        }
    }
}