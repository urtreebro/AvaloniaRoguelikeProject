using Avalonia;

using ReactiveUI;

namespace AvaloniaRoguelike.Model;

public abstract class GameObject : ReactiveObject
{
    private Point _location;
    private CellLocation _cellLocation;

    public Point Location
    {
        get { return _location; }
        protected set
        {
            this.RaiseAndSetIfChanged(ref _location, value);
        }
    }
    public virtual CellLocation CellLocation
    {
        get { return _cellLocation; }
        protected set
        {
            this.RaiseAndSetIfChanged(ref _cellLocation, value);
            Location = CellLocation.ToPoint();
        }
    }

    public virtual int Layer => 0;

    public bool IsInRange(CellLocation otherCell, int sightRadius)
    {
        return (CellLocation.X - otherCell.X) * (CellLocation.X - otherCell.X)
             + (CellLocation.Y - otherCell.Y) * (CellLocation.Y - otherCell.Y) <= sightRadius * sightRadius;
    }

    protected GameObject(Point location)
    {
        Location = location;
    }

    protected GameObject(CellLocation cellLocation)
    {
        CellLocation = cellLocation;
    }
}