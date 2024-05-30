using Avalonia;

using ReactiveUI;

namespace AvaloniaRoguelike.Model;

public sealed class Camera : ReactiveObject
{
    private Vector _offset;
    public const int Width = 24;
    public const int Height = 16;

    public Camera()
    {
        _offset = new Vector(0,0);
    }

    public Vector Offset
    {
        get { return _offset; }
        set
        {
            this.RaiseAndSetIfChanged(ref _offset, value);
        }
    }

    public void ReCalculateVisibleObjects(Point playerLocation)
    {
        double offsetX = playerLocation.X / 2;
        double offsetY = playerLocation.Y / 2;
        Offset = new Vector(offsetX, offsetY);
    }
}