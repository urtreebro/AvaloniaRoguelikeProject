using Avalonia.Threading;
using ReactiveUI;
using System;
using ReactiveUI;
using AvaloniaRoguelike.ViewModels;

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

        public int Level { get; protected set; }

        public void Start() => _timer.IsEnabled = true;
        public void Stop() => _timer.IsEnabled = false;
    }
}
