using AvaloniaRoguelike.Model;

using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
{
    private Game _game;
    private GameField _gameField;

    public MainWindowViewModel(
        Game game,
        GameField gameField)
    {
        _game = game;
        _gameField = gameField;
        //ButtonClickCommand = ReactiveCommand.Create(ButtonClick);
    }

    public GameField GameField
    {
        get => _gameField;
        set
        {
            this.RaiseAndSetIfChanged(ref _gameField, value);
        }
    }

    //public void ButtonClick() => GameField.TempValue = "RogueLike";
    //public ReactiveCommand<Unit, Unit> ButtonClickCommand { get; }
}
}