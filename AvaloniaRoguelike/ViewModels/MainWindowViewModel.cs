using AvaloniaRoguelike.Model;
using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Game _game;

    public MainWindowViewModel(
        Game game)
    {
        _game = game;
    }

    public Game Game
    {
        get => _game;
        set
        {
            this.RaiseAndSetIfChanged(ref _game, value);
        }
    }
}