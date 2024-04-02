using Avalonia;

using AvaloniaRoguelike.Model;

using ReactiveUI;

using System.Reactive;

namespace AvaloniaRoguelike.ViewModels;

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
            // TODO: масштабировать эту строчку на остальные свойства с OnPropertyChanged
            this.RaiseAndSetIfChanged(ref _gameField, value);
        }
    }

    //public void ButtonClick() => GameField.TempValue = "RogueLike";
    //public ReactiveCommand<Unit, Unit> ButtonClickCommand { get; }
}
