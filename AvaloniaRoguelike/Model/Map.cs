using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.Model
{
    public class Map
    {
        private const int _width = 32;
        private const int _height = 24;
        private const int _roomMinSize = 4;
        private const int _roomMaxSize = 15;
        private Random _random = new Random();
        
        private string[,] _map;

        public Map()
        {
            _map = new string[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _map[x, y] = "0";
                }
            }
            CreateRooms();
            CreateWalls();
        }

        public string this[int x, int y]
        {
            get { return _map[x, y]; }
        }

        public string[,] CreateRooms()
        {
            var rooms = new List<Room>();

            for (int r = 0; r < 100; r++)
            {
                int roomWidth = _random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = _random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = _random.Next(0, _width - roomWidth - 1);
                int roomYPosition = _random.Next(0, _height - roomHeight - 1);

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
            foreach (Room room in rooms)
            {
                MakeRoom(room);
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
                    MakeHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    MakeVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    MakeVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    MakeHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }
            return _map;
        }

        private string[,] CreateWalls()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_map[x, y] == "0") 
                    {
                        if (x > 0 && _map[x - 1, y] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (x < _width - 1 && _map[x + 1, y] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (y > 0 && _map[x, y - 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (y < _height - 1 && _map[x, y + 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (x > 0 && y > 0 && _map[x - 1, y - 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (x < _width - 1 && y > 0 && _map[x + 1, y - 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (x > 0 && y < _height - 1 && _map[x - 1, y + 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                        else if (x < _width - 1 && y < _height - 1 && _map[x + 1, y + 1] == ".")
                        {
                            _map[x, y] = "#";
                        }
                    }
                }
            }
            return _map;
        }
        private void MakeRoom(Room room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map[x, y] = ".";
                }
            }
        }

        private void MakeHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map[x, yPosition] = ".";
            }
        }

        private void MakeVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map[xPosition, y] = ".";
            }
        }
    }

    public struct Room
    {
        public int Height;
        public int Width;
        public int X;
        public int Y;

        public int Left => X;

        public int Right => X + Width;

        public int Top => Y;

        public int Bottom => Y + Height;

        public Room(int x, int y, int width, int height)
        {
            X = x; Y = y; Width = width; Height = height;
        }
        public bool Intersects(Room value)
        {
            return value.Left < Right &&
                   Left < value.Right &&
                   value.Top < Bottom &&
                   Top < value.Bottom;
        }

        public Point Center => new Point(X + (Width / 2), Y + (Height / 2));
    }
}
