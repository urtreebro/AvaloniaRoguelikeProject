using Avalonia;

namespace AvaloniaRoguelike.Model;

// TODO: evma, rename to MapCell?
public struct CellLocation
{
    public CellLocation(int x, int y)
    {
        X = x;
        Y = y;
    }

    public CellLocation((int x, int y) coords)
    {
        X = coords.x;
        Y = coords.y;
    }

    public int X { get; }
    public int Y { get; }

    public readonly CellLocation WithX(int x)
        => new(x, Y);

    public readonly CellLocation WithY(int y)
        => new(X, y);

    public readonly CellLocation WithXPlus(int xDelta)
        => new(X + xDelta, Y);

    public readonly CellLocation WithYPlus(int yDelta)
        => new(X, Y + yDelta);

    public readonly CellLocation WithXMinus(int xDelta)
        => new(X - xDelta, Y);

    public readonly CellLocation WithYMinus(int yDelta)
        => new(X, Y - yDelta);

    public readonly bool Equals(CellLocation other)
    {
        return X == other.X && Y == other.Y;
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is null) 
            return false;
        return obj is CellLocation location && Equals(location);
    }

    public static bool operator ==(CellLocation l1, CellLocation l2) 
        => l1.Equals(l2);

    public static bool operator !=(CellLocation l1, CellLocation l2) 
        => !(l1 == l2);

    public override int GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public override readonly string ToString() 
        => $"({X}:{Y})";

    public readonly Point ToPoint()
        => new(GameField.CellSize * X, GameField.CellSize * Y);
}
