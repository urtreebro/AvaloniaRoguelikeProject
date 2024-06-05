using AvaloniaRoguelike.Model;
using ReactiveUI;

using System;

namespace AvaloniaRoguelike.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Game _game;

    public MainViewModel()
    {
        _game = new Game();
    }

    public Game Game
    {
        get => _game;
        set
        {
            this.RaiseAndSetIfChanged(ref _game, value);
        }
    }

    public void StartGame()
    {
        _game.Start();
        _game.Camera.MoveCamera(_game.Player.Location);
    }

    public void StopGame()
    {
        _game.Stop();
    }

    public void SetPlayerName(string name)
    {
        ArgumentNullException.ThrowIfNull(_game, nameof(_game));
        _game.SetPlayerName(name);
    }
}