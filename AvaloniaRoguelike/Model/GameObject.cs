using System;
using Avalonia;
using AvaloniaRoguelike.Infrastructure;
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
                if (value.Equals(_location))
                    return;
                _location = value;
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
