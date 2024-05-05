using Avalonia;

namespace AvaloniaRoguelike.Model;

public static class PointExtensions
{
    public static CellLocation ToCellLocation(this Point point)
    {
        return new CellLocation((int)(point.X / GameField.CellSize), (int)(point.Y / GameField.CellSize));
    }
}