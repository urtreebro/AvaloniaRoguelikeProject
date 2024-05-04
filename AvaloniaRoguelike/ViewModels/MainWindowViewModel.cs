using AvaloniaRoguelike.Model;
using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Game _game;
        private ViewModelBase content;

        public MainWindowViewModel(
            Game game)
        {
            Content = new MainViewModel(game);
            Game = game;
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
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        //public void ButtonClick() => GameField.TempValue = "RogueLike";
        //public ReactiveCommand<Unit, Unit> ButtonClickCommand { get; }
    }
}