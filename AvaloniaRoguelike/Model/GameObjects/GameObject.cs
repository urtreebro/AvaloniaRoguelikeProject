using Avalonia;

using ReactiveUI;

namespace AvaloniaRoguelike.Model
{
    public abstract class GameObject : ReactiveObject
    {
        private Point _location;

        public Point Location
        {
            get { return _location; }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _location, value);
            }
        }

        public virtual int Layer => 0;

        protected GameObject(Point location)
        {
            Location = location;
        }
    }
}
