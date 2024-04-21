using Avalonia.Threading;
using System;
using ReactiveUI;

namespace AvaloniaRoguelike.Model
{
    public abstract class GameBase : ReactiveObject
    {
        private readonly DispatcherTimer _timer = new() { Interval = new TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond) };

        protected GameBase()
        {
            _timer.Tick += delegate { DoTick(); };
        }

        private void DoTick()
        {
            Tick();
            CurrentTick++;
        }

        protected abstract void Tick();

        public const int TicksPerSecond = 60;
        public long CurrentTick { get; private set; }

        public void Start() => _timer.IsEnabled = true;
        public void Stop() => _timer.IsEnabled = false;
    }
}
