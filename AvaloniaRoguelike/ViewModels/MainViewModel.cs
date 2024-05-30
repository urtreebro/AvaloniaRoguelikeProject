using AvaloniaRoguelike.Model;
using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Game _game;

    public MainViewModel()
    {
        var field = new GameField(0);
        _game = new Game(field);
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
        _game.Camera.ReCalculateVisibleObjects(_game.Player.CellLocation);
    }
}