﻿using Avalonia.Threading;
using System;

namespace AvaloniaRoguelike.Model
{
    public abstract class GameBase
    {
        public const int TicksPerSecond = 60;
        public long CurrentTick { get; private set; }
        private readonly DispatcherTimer _timer = new() { Interval = new TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond) };


        void DoTick()
        {
            Tick();
            CurrentTick++;
        }

        protected abstract void Tick();

        protected GameBase()
        {
            _timer.Tick += delegate { DoTick(); };
        }

        public void Start() => _timer.IsEnabled = true;
        public void Stop() => _timer.IsEnabled = false;
    }
}