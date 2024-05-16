using Avalonia.Threading;
using ReactiveUI;
using System;
using ReactiveUI;
using AvaloniaRoguelike.ViewModels;

namespace AvaloniaRoguelike.Model;

public abstract class GameBase : ReactiveObject
{
    private readonly DispatcherTimer _timer = new() { Interval = new TimeSpan(0, 0, 0, 0, (int)DeltaTime) };

    protected GameBase()
    {
        _timer.Tick += delegate { DoTick(); };
    }

    private void DoTick()
    {
        try
        {
            Tick();
            CurrentTick++;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    protected abstract void Tick();

    public const int TicksPerSecond = 60;
    public static double DeltaTime = 1000 / TicksPerSecond;
    public long CurrentTick { get; private set; }
    public int Lvl { get; protected set; }

    public void Start() => _timer.IsEnabled = true;
    public void Stop() => _timer.IsEnabled = false;
}