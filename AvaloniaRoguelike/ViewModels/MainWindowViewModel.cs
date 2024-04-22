using AvaloniaRoguelike.Model;
using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Game _game;

        public MainWindowViewModel(
            Game game)
        {
            _game = game;
            //ButtonClickCommand = ReactiveCommand.Create(ButtonClick);
        }

        public Game Game
        {
            get => _game;
            set
            {
                this.RaiseAndSetIfChanged(ref _game, value);
            }
        }

        //public void ButtonClick() => GameField.TempValue = "RogueLike";
        //public ReactiveCommand<Unit, Unit> ButtonClickCommand { get; }
    }
}