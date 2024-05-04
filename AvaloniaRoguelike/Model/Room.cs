namespace AvaloniaRoguelike.Model;

public struct Room(int x, int y, int width, int height)
{
    public int Height = height;
    public int Width = width;
    public int X = x;
    public int Y = y;

    public int Left => X;

    public int Right => X + Width;

    public int Top => Y;

    public int Bottom => Y + Height;

    public bool Intersects(Room value)
    {
        return value.Left < Right &&
               Left < value.Right &&
               value.Top < Bottom &&
               Top < value.Bottom;
    }

    public CellLocation Center => new CellLocation(X + (Width / 2), Y + (Height / 2));
}
