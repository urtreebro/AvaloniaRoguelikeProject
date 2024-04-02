using System;
using Avalonia;
using AvaloniaRoguelike.Infrastructure;

namespace AvaloniaRoguelike.Model
{
    public abstract class GameObject : PropertyChangedBase
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
                OnPropertyChanged();
            }
        }

        public virtual int Layer => 0;

        protected GameObject(Point location)
        {
            Location = location;
        }
    }
}
