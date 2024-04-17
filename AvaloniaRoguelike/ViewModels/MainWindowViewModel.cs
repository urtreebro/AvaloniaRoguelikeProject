using AvaloniaRoguelike.Model;

using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels
{

public class MainWindowViewModel : ViewModelBase
{
    private Game _game;
    private GameField _gameField;

    public MainWindowViewModel(
        Game game)
    {
        _game = game;
        //ButtonClickCommand = ReactiveCommand.Create(ButtonClick);
    }

    public GameField GameField
    {
        get => _game.Field;
        set
        {
            this.RaiseAndSetIfChanged(ref _game.Field, value);
        }
    }

    //public void ButtonClick() => GameField.TempValue = "RogueLike";
    //public ReactiveCommand<Unit, Unit> ButtonClickCommand { get; }
}
}