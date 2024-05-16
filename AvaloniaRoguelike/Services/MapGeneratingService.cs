using AvaloniaRoguelike.Model;

using System;
using System.Collections.Generic;

namespace AvaloniaRoguelike.Services;

public class MapGeneratingService : IMapGeneratingService
{
    /* Old rules
        if (Map[x, y] == ".") 
            return TerrainTileType.Plain;
        else if (Map[x, y] == "#") 
            return TerrainTileType.Wall;
        return TerrainTileType.Background;
     */
    private readonly Random _random = new();
    private const int _roomMinSize = 4;
    private const int _roomMaxSize = 15;

    public void InitializeMap(GameField gameField)
    {
        for (int x = 0; x < gameField.Width; x++)
        {
            for (int y = 0; y < gameField.Height; y++)
            {
                gameField[x, y] = new TerrainTile(new CellLocation(x, y));
            }
        }
        CreateRooms(gameField);
        CreateWalls(gameField);
    }

    public void CreateRooms(GameField gameField)
    {
        var rooms = new List<Room>();

        for (int r = 0; r < 100; r++)
        {
            int roomWidth = _random.Next(_roomMinSize, _roomMaxSize);
            int roomHeight = _random.Next(_roomMinSize, _roomMaxSize);
            int roomXPosition = _random.Next(0, gameField.Width - roomWidth - 1);
            int roomYPosition = _random.Next(0, gameField.Height - roomHeight - 1);

            var newRoom = new Room(roomXPosition, roomYPosition, roomWidth, roomHeight);

            bool newRoomIntersects = false;

            foreach (Room room in rooms)
            {
                if (newRoom.Intersects(room))
                {
                    newRoomIntersects = true;
                    break;
                }
            }
            if (!newRoomIntersects)
            {
                rooms.Add(newRoom);
            }
        }
        foreach (var room in rooms)
        {
            MakeRoom(gameField, room);
        }

        for (int r = 0; r < rooms.Count; r++)
        {
            if (r == 0)
            {
                continue;
            }

            int previousRoomCenterX = rooms[r - 1].Center.X;
            int previousRoomCenterY = rooms[r - 1].Center.Y;
            int currentRoomCenterX = rooms[r].Center.X;
            int currentRoomCenterY = rooms[r].Center.Y;

            if (_random.Next(0, 2) == 0)
            {
                MakeHorizontalTunnel(gameField, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                MakeVerticalTunnel(gameField, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
            }
            else
            {
                MakeVerticalTunnel(gameField, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                MakeHorizontalTunnel(gameField, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
            }
        }
    }

    public void CreateWalls(GameField gameField)
    {
        for (int x = 0; x < gameField.Width; x++)
        {
            for (int y = 0; y < gameField.Height; y++)
            {
                if (gameField[x, y].Type == TerrainTileType.Background)
                {
                    if (x > 0 && gameField[x - 1, y].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (x < gameField.Width - 1 && gameField [x + 1, y].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (y > 0 && gameField[x, y - 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (y < gameField.Height - 1 && gameField[x, y + 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (x > 0 && y > 0 && gameField[x - 1, y - 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (x < gameField.Width - 1 && y > 0 && gameField[x + 1, y - 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (x > 0 && y < gameField.Height - 1 && gameField[x - 1, y + 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                    else if (x < gameField.Width - 1 && y < gameField.Height - 1 && gameField[x + 1, y + 1].Type == TerrainTileType.Plain)
                    {
                        gameField[x, y].Type = TerrainTileType.Wall;
                    }
                }
            }
        }
    }

    public void MakeRoom(GameField gameField, Room room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top + 1; y < room.Bottom; y++)
            {
                gameField[x, y].Type = TerrainTileType.Plain;
            }
        }
    }

    public void MakeHorizontalTunnel(GameField gameField, int xStart, int xEnd, int yPosition)
    {
        for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        {
            gameField[x, yPosition].Type = TerrainTileType.Plain;
        }
    }

    public void MakeVerticalTunnel(GameField gameField, int yStart, int yEnd, int xPosition)
    {
        for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        {
            gameField[xPosition, y].Type = TerrainTileType.Plain;
        }
    }
}