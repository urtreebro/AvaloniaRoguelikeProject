using Avalonia;

using ReactiveUI;

using System.Collections.ObjectModel;

namespace AvaloniaRoguelike.Model;

// TODO: MakarovEA, сейчас _visibleGameObjects по факту задает поле зрения персонажа, игнорируя непроходимость стен
public sealed class Camera : ReactiveObject
{
    private GameField _field;
    private Vector _offset;
    //private ObservableCollection<GameObject> _visibleGameObjects;
    public const int Width = 24;
    public const int Height = 16;

    public Camera(GameField field)
    {
        _field = field;
        //_visibleGameObjects = [];
        _offset = new Vector(_field.Player.Location.X, _field.Player.Location.Y);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public ObservableCollection<GameObject> GameObjects
    //{
    //    get { return _visibleGameObjects; }
    //    set
    //    {
    //        this.RaiseAndSetIfChanged(ref _visibleGameObjects, value);
    //    }
    //}
    public Vector Offset
    {
        get { return _offset; }
        set
        {
            this.RaiseAndSetIfChanged(ref _offset, value);
        }
    }

    public void ReCalculateVisibleObjects(CellLocation center)
    {
        //_visibleGameObjects.Clear();

        //foreach (var go in _field.GameObjects)
        //{
        //    if (go.CellLocation.X >= Math.Max(0, center.X - Width / 2) &&
        //        go.CellLocation.X <= Math.Min(_field.Width, center.X + Width / 2) &&
        //        go.CellLocation.Y >= Math.Max(0, center.Y - Height / 2) &&
        //        go.CellLocation.Y <= Math.Min(_field.Height, center.Y + Height / 2))
        //    {
        //        _visibleGameObjects.Add(go);
        //    }
        //}
        //double offsetX = _field.Player.Location.X + Width / 2 * GameField.CellSize;
        //double offsetY = _field.Player.Location.Y + Height / 2 * GameField.CellSize;
        double offsetX = _field.Player.Location.X / 2;
        double offsetY = _field.Player.Location.Y / 2;
        Offset = new Vector(offsetX, offsetY);
    }
}